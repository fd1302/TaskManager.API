async function addBoard(id) {
    const addDto = {
        description: document.getElementById("boardDescription").value
    };
    try {
        const response = await fetch(`${boardUrl}/add?projectId=${id}`, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(addDto)
        });
        if(response.ok) {
            alert("Board added.");
        } else {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function updateBoard(id, projectId) {
    const updateDto = {
        description: document.getElementById("boardDescription").value
    };
    try {
        const response = await fetch(`${boardUrl}/updateboard?id=${id}`, {
            method: "PUT",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(updateDto)
        });
        const result = await response.json();
        if(response.ok) {
            alert(result.message);
            getBoardsWithProjectId(projectId);
        } else if(!response.ok) {
            throw new Error(`There was a probel while handling the request: ${result.message}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}