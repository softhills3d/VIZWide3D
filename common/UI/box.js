class Box {
    constructor() {
        // Main div
        let div = document.createElement('div');
        div.className = "position-absolute d-flex justify-content-center end-0 bg-light bg-opacity-50 rounded";
        div.style.zIndex = "3";
        div.style.width = "350px";
        div.style.height = "500px";
        div.style.marginTop = "150px";
        div.style.marginRight = "8.5px";
        document.body.appendChild(div);

        // Sub div
        let sub_div = document.createElement('div');
        sub_div.className = "d-flex flex-md-column"
        sub_div.style.width = "270px";
        sub_div.style.paddingTop = "20px";
        div.appendChild(sub_div);

        // Button div
        let btn_div = document.createElement('div');
        btn_div.className = "d-flex flex-md-column";
        sub_div.appendChild(btn_div);

        // Check div (column)
        let check_div = document.createElement('div');
        check_div.className = "d-flex flex-md-column"
        sub_div.appendChild(check_div);

        
        // Globla Var
        this.btn_div = btn_div;
        this.check_div = check_div;
    }

    CreateButton(id, text) {
        let button = document.createElement('button');
        button.id = id;
        button.className = "btn btn-dark mb-3";
        button.textContent = text

        this.btn_div.appendChild(button);
    }
    
    CreateCheckBox(id, text) {
        // Check Sub div (row 2)
        let check_sub_div = document.createElement('div');
        check_sub_div.className = "d-flex flex-md-row"

        let label = document.createElement('label');
        label.htmlFor = id;
        label.innerText= text

        let checkBox = document.createElement('input');
        checkBox.id = id;
        checkBox.className = "form-check-input mb-3";
        checkBox.type = "checkbox";
        checkBox.name = id;
        checkBox.style.marginRight = "5px";
        
        check_sub_div.appendChild(checkBox);
        check_sub_div.appendChild(label);

        this.check_div.appendChild(check_sub_div);
    }
}

export default Box;