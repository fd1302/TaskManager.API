async function updateTaskItem(id) {
    const updateDto = {
        title: document.getElementById("taskItemTitle").value,
        description: document.getElementById("taskItemDescription").value,
        status: document.getElementById("taskItemStatus").value
    };
    try {
        const response = await fetch(`${taskItemUrl}/updatetaskitem?id=${id}`, {
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
        } else if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}