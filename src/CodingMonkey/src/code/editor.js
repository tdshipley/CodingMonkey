import {inject} from 'aurelia-framework';
import {HttpClient, json} from 'aurelia-fetch-client';
import 'fetch';
import 'ace';

@inject(HttpClient)
export class Editor {
    constructor(http) {
        http.configure(config => {
            config
                .useStandardConfiguration()
                .withBaseUrl('http://localhost:9000/api/CodeExecution/');
        });

        this.http = http;
        this.vm = this;
        this.markedLines = [];
    }

    // Run this code after page has loaded
    attached() {
        //Config for ace - https://github.com/jspm/registry/issues/38#issuecomment-168572405
        let base = System.normalizeSync('ace');
        base = base.substr(0, base.length - 3);
        ace.config.set('basePath', base);

        //Ace settings
        this.codeEditor = ace.edit("aceEditor");
        this.codeEditor.setTheme("ace/theme/monokai");
        this.codeEditor.getSession().setMode("ace/mode/csharp");

        //Set Value
        this.codeEditor.setValue("public class Numbers  \n{\n    public int ReturnNumber6()\n    {\n        return 6;\n    }\n}", -1);
    }

    submitCodeToExecute() {
        this.http.fetch('Execute', {
            method: 'post',
            body: json({code: this.codeEditor.getValue()})
        })
        .then(response => response.json())
        .then(data => {
            this.vm = data;
            this.vm.SubmittedCode = true;
        })
        .catch(err => {
            console.log(err);
        });
    }

    submitCodeToCompile() {
        this.http.fetch('Compile', {
            method: 'post',
            body: json({code: this.codeEditor.getValue()})
        })
        .then(response => response.json())
        .then(data => {
            this.vm = data;
            this.vm.SubmittedCode = true;

            this.highlightErrors(data);
        })
        .catch(err => {
            console.log(err);
        });
    }

    highlightErrors(data) {
        if (data.HasErrors)
        {
            for (let error of data.Errors) {
                this.highlightError(error.LineNumberStart, error.LineNumberEnd, 0, error.ColEnd);
            }
        }
        else
        {
            this.unhighlightError();
        }
    }

    highlightError(startLine, endLine, startCol, endCol) {
        this.unhighlightError();
        var Range = ace.require("ace/range").Range
        this.markedLines.push(this.codeEditor.session.addMarker(new Range(startLine - 1, startCol, endLine - 1, endCol), "errorHighlight", "fullLine"));
    }

    unhighlightError() {
        for (var i = 0; i < this.markedLines.length; i++) {
            this.codeEditor.getSession().removeMarker(this.markedLines[i]);
        }

        this.markedLines = [];
    }
}