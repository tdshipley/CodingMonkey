export class Authentication {
    constructor(router) {
        this.appRouter = router;
    }

    verifyUserLoggedIn() {
        var currentUser = Authentication.getCurrentUserInSessionStorage();

        if (!currentUser.isLoggedIn) {
            this.appRouter.navigate("/");
        }
    }

    static getCurrentUserInSessionStorage() {
        let currentUserRaw = sessionStorage.getItem("currentUser");
        let currentUser = JSON.parse(currentUserRaw);

        return currentUser;
    }
}