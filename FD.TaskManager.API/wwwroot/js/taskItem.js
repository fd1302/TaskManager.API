async function addTaskItem(id) {
    const addDto = {
        boardId: id,
        title: document.getElementById("taskItemTitle").value,
        description: document.getElementById("taskItemDescription").value,
        assignedMemberId: document.getElementById("membersForTask").value
    }
    console.log(addDto.boardId);
    try {
        const response = await fetch(`${taskItemUrl}/add`, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(addDto)
        });
        if(response.ok) {
            alert("Added successfuly.");
        } else {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function getTaskItemsWithMemberId() {
    try {
        const response = await fetch(`${taskItemUrl}/gettaskitemswithmemberid`);
        const result = await response.json();
        if(response.ok) {
            return result;
        } else {
            throw new Error(`There was a problem while handling the request: ${result.message} - ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

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

async function deleteTaskItem(id, boardId) {
    const response = await fetch(`${taskItemUrl}/delete?id=${id}`, {
        method: "DELETE"
    });
    if(response.ok) {
        const tasks = getTasksWithBoardId(boardId);
        const tasksEmpty = jsonIsEmpty(tasks);
        if(tasksEmpty) {
            window.location.reload();
        }
    } else {
        throw new Error(`There was a problem while handling the request: ${response.status}`);
    }
}