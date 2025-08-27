async function updateBoard(id) {
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
        } else if(!response.ok) {
            throw new Error(`There was a probel while handling the request: ${result.message}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}