async function addProject() {
    const addDto = {
        name: document.getElementById("projectName").value,
        description: document.getElementById("projectDescription").value
    };
    try {
        const response = await fetch(`${projectUrl}/add`, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(addDto)
        })
        const result = await response.json();
        if(response.ok) {
            alert(result.id);
        }
        if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function updateProject(id) {
    const updateDto = {
        name: document.getElementById("projectName").value,
        description: document.getElementById("projectDescription").value
    };
    try {
        const response = await fetch(`${projectUrl}/update?id=${id}`, {
            method: "PATCH",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(updateDto)
        });
        const result = await response.json();
        if(response.ok) {
            alert(result.message);
            showProjects();
        }else if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${result.message}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}