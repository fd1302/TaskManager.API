async function updateProject(id) {
    const updateDto = {
        name: document.getElementById("projectName").value,
        description: document.getElementById("projectDescription").value
    };
    try {
        const response = await fetch(`${projectUrl}/updateproject?id=${id}`, {
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
        }else if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${result.message}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}