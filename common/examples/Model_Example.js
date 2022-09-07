import Box from '../UI/box.js';

class Mooel_Example {
    constructor(vizwide3d) {
        this.vizwide3d = vizwide3d;
        
        // API Example
        

        // UI 생성
        {
            let box = new Box();
            let buttons = [
                "Model Open",
                "[ Popup ] Open",
                "Add Model",
                "Close",
                "Bound-Box",
                "Files"
            ];
            buttons.forEach(value => {
                box.CreateButton(value, value);
                document.getElementById(value).addEventListener('click', onTest)
            })

            let checkboxs = [ 
                "OnModelOpenedEvent",
                "OnPropertyCompletedEvent",
                "OnStreamProgressChangedEvent",
                "OnStructureCompletedEvent"
                ];
            checkboxs.forEach(value => {
                box.CreateCheckBox(value, value);
            })
        
            function onTest(e) {
                const id = e.srcElement.id;
                if (id === buttons[0]) {
                    // Model Open
                    console.log('Model Open');
                } else if (id === buttons[1]) {
                    console.log('[ Popup ] Open');
                } else if (id === buttons[2]) {
                    console.log('Add Model');
                } else if (id === buttons[3]) {
                    console.log('Close');
                } else if (id === buttons[4]) {
                    console.log('Bound-Box');
                } else if (id === buttons[5]) {
                    console.log('Files');
                }
            }
        }
    }

}

export default Mooel_Example;