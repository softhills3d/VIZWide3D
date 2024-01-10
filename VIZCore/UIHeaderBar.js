class UIHeaderBar {
    constructor(vizcore) {
        let view = document.getElementById(vizcore.Main.GetViewID());
        let scope = this;

        this.headerBar  = undefined;

        function initHeader() {

            let headerBar = document.createElement('div');
            headerBar.id = vizcore.Main.GetViewID() +"headerBar"
            headerBar.className = "headerBar";

            view.parentNode.appendChild(headerBar);
            
            scope.headerBar = headerBar;
            view.style.top = "50px";
            view.style.height = "calc(100% - 50px)";

            vizcore.Main.Resize();
            vizcore.View.ResizeGLWindow();
            vizcore.Render();
        }

        function createButton(text, clickHandler) {
            let button = document.createElement('div');
            button.className = "ui_button_header";
            button.id =  text;
            button.innerHTML = text;
            button.style.float = 'left';
            button.addEventListener('click', clickHandler);

            return button;
        }

        this.Init = function () {
            initHeader();
        };

        this.AddButton = function(text, clickHandler){
            let button = createButton(text,clickHandler);
            scope.headerBar.appendChild(button);
        }

    }
};

export default UIHeaderBar;
