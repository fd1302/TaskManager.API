async function addManager(event) {
    event.preventDefault();
    const addManagerDto = {
        userName: document.getElementById("username").value,
        password: document.getElementById("password").value,
        email: document.getElementById("email").value,
        tenantId: document.getElementById("tenantSelect").value
    }
    try {
        const response = await fetch(`${managerUrl}/addmanager`, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(addManagerDto)
        });
        if (response.ok) {
            alert("Sign up complete!");
            window.location.href = "account.html";
        }
        else if (!response.ok) {
            throw new Error(`There was a problem while handling the request: ${await response.json()}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function getManagersWithTenantId() {
    try {
        const response = await fetch(`${managerUrl}/getmanagerswithtenantid`);
        if(response.ok) {
            return await response.json();
        } else if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function updateManager() {
    const updateDto = {
        userName: document.getElementById("managerUserName").value,
        email: document.getElementById("managerEmail").value
    };
    try {
        const response = await fetch(`${managerUrl}/updateManager`, {
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
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function promoteToManager(id) {
    const updateDto = {
        userId: id,
        role: "Manager"
    };
    try {
        const response = await fetch(`${managerUrl}/promotion`, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(updateDto)
        });
        if(response.ok) {
            tenantMembers();
        } else if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function demoteToManager(id) {
    const updateDto = {
        userId: id,
        role: "Manager"
    };
    try {
        const response = await fetch(`${managerUrl}/demotion`, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(updateDto)
        });
        if(response.ok) {
            showAdmins();
        } else if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}