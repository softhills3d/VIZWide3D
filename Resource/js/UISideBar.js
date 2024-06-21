class UISideBar {
    constructor(vizcore) {
        let contain = document.getElementsByClassName("VIZWeb_Main")[0];
        let scope = this;

        this.sideBar = undefined;

        function initSider() {
            let sideBar = document.createElement('div');
            sideBar.id = vizcore.Main.GetViewID() + "sideBar"
            sideBar.className = "sideBar";
            sideBar.style.backgroundColor = (245, 245, 245);

            contain.parentNode.appendChild(sideBar);

            scope.sideBar = sideBar;
            contain.style.left = "150px";
            contain.style.width = "calc(100% - 150px)";

            vizcore.Main.Resize();
            vizcore.View.ResizeGLWindow();
            vizcore.Render();
        }

        function createBtn(text, clickHandler) {
            let button = document.createElement('div');
            button.className = "ui_button_side";
            button.id = text;
            button.innerHTML = text;
            button.style.float = 'left';
            button.style.fontSize = "14px";
            button.addEventListener('click', clickHandler);

            return button;
        }

        function createTitle(text) {
            let title = document.createElement('div');
            title.id = vizcore.Main.GetViewID() + "title"
            title.innerHTML = text;
            title.style.float = 'left';
            title.style.fontSize = "14px";
            title.className = "ui_title";

            return title;
        };

        this.Init = function () {
            initSider();
        };

        this.AddButton = function (text, clickHandler) {
            let button = createBtn(text, clickHandler);
            scope.sideBar.appendChild(button);
        }

        this.AddTitle = function (text) {
            let title = createTitle(text);
            scope.sideBar.appendChild(title);
        }

    }
};

export default UISideBar;
