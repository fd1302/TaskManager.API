async function addMember(event) {
    event.preventDefault();
    const addMemberDto = {
        userName: document.getElementById("username").value,
        password: document.getElementById("password").value,
        email: document.getElementById("email").value
    };
    try {
        const response = await fetch(`${memberUrl}/addmember`, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(addMemberDto)
        });
        if (response.ok) {
            alert("Sign up complete!");
            window.location.href = "profile.html";
        }
        else if (!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function getMembersWithTenantId() {
    try {
        const response = await fetch(`${memberUrl}/getmemberswithtenantid`);
        if(response.ok) {
            return await response.json();
        } else if(!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function getMembers() {
    try {
        const response = await fetch(`${memberUrl}/getmembers`);
        if(!response.ok) {
            throw new Error(`There was a problem while fetching data: ${response.status}`);
        }
        return await response.json();
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function demoteToMember(id) {
    const updateDto = {
        userId: id,
        role: "Member"
    };
    try {
        const response = await fetch(`${memberUrl}/demotion`, {
        method: "PATCH",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(updateDto)
        })
        if(response.ok) {
            showManagers();
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function updateMember() {
    const updateDto = {
        userName: document.getElementById("memberUserName").value,
        email: document.getElementById("memberEmail").value
    };
    try {
        const response = await fetch(`${memberUrl}/update`, {
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
        } else {
            throw new Error(`There was a problem while handling the request: ${result.message} - ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function removeTenantMember(id) {
    const updateDto = {
        userId: id,
        role: "Members"
    };
    try {
        const response = await fetch(`${memberUrl}/removetenantmember`, {
            method: "PATCH",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(updateDto)
        });
        if(response.ok) {
            tenantMembers();
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}