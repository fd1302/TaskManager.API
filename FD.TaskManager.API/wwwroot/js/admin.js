async function getAdminsWithTenantId() {
    try {
        const response = await fetch(`${adminUrl}/getadminswithtenantid`);
        if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
        const result = await response.json();
        return result;
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function updateAdmin() {
    const updateDto = {
        userName: document.getElementById("adminUserName").value,
        email: document.getElementById("adminEmail").value
    };
    console.log(updateDto.email, updateDto.userName);
    try {
        const response = await fetch(`${adminUrl}/updateadmin`, {
            method: "PATCH",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(updateDto)
        });
        const result = await response.json();
        if(!response.ok) {
            alert(`Update failed: ${result.message}`);
            throw new Error(`There was a problem while handling the request: ${result.message}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function promoteToAdmin(id) {
    const updateDto = {
        userId: id,
        role: "Admin"
    };
    try {
        const response = await fetch(`${adminUrl}/promotion`, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(updateDto)
        })
        if(response.ok) {
            showManagers();
        } else if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}