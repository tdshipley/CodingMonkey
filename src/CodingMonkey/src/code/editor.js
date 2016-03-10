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
    }

    submitCodeToCompile() {
        this.http.fetch('Compile', {
            method: 'post',
            body: json({code: this.codeEditor.getValue()})
        })
        .then(response => response.json())
        .then(data => {
            console.log(data);
        })
        .catch(err => {
            console.log(err);
        });
    }
}