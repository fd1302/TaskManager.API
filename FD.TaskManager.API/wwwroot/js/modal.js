const modal = document.getElementById("profileModal");
const modalHeader = document.getElementById("modalHeader");
const modalBody = document.getElementById("modalBody");
const modalFooter = document.getElementById("modalFooter");
const closeModal = document.getElementById("closeModal");

function openUpdateModal(id, modalType, entity) {
    if(modalType === "project") {
        updateProjectModalStyle(id, entity);
    } else if(modalType === "board") {
        updateBoardModalStyle(id, entity);
    } else if(modalType === "taskItem") {
        updateTaskItemModalStyle(id, entity);
    } else {
        return "Modal type has wrong value";
    }
    closeModal.onclick = function() { modal.style.display = "none"};
}

// Projects
function updateProjectModalStyle(id, project) {
    modal.style.display = "block";
    modalHeader.innerHTML = "";
    modalBody.innerHTML = "";
    modalFooter.innerHTML = "";
    const title = document.createElement("h2");
    title.textContent = "Edit project";
    modalHeader.appendChild(title);
    const nameLabel = document.createElement("label");
    nameLabel.textContent = "Name: ";
    modalBody.appendChild(nameLabel);
    const nameInput = document.createElement("input");
    nameInput.type = "text";
    nameInput.value = project.name;
    nameInput.id = "projectName";
    modalBody.appendChild(nameInput);
    const descriptionLabel = document.createElement("label");
    descriptionLabel.textContent = "Description: ";
    modalBody.appendChild(descriptionLabel);
    const descriptionTextarea = document.createElement("textarea");
    descriptionTextarea.type = "text";
    descriptionTextarea.value = project.description;
    descriptionTextarea.id = "projectDescription";
    modalBody.appendChild(descriptionTextarea);
    const updateBtn = document.createElement("button");
    updateBtn.textContent = "Edit";
    updateBtn.onclick = function() { updateProject(id) };
    modalFooter.appendChild(updateBtn);
}

// Boards
function addBoardModal(id) {
    modal.style.display = "block";
    modalHeader.innerHTML = "";
    modalBody.innerHTML = "";
    modalFooter.innerHTML = "";
    const title = document.createElement("h2");
    title.textContent = "Add Board";
    modalHeader.appendChild(title);
    const descriptionLabel = document.createElement("label");
    descriptionLabel.textContent = "Description: ";
    modalBody.appendChild(descriptionLabel);
    const descriptionTextarea = document.createElement("textarea");
    descriptionTextarea.id = "boardDescription";
    modalBody.appendChild(descriptionTextarea);
    const updateBtn = document.createElement("button");
    updateBtn.textContent = "Add";
    updateBtn.onclick = function() { addBoard(id) };
    modalFooter.appendChild(updateBtn);
}

function updateBoardModalStyle(id, board) {
    modal.style.display = "block";
    modalHeader.innerHTML = "";
    modalBody.innerHTML = "";
    modalFooter.innerHTML = "";
    const title = document.createElement("h2");
    title.textContent = "Edit board";
    modalHeader.appendChild(title);
    const descriptionLabel = document.createElement("label");
    descriptionLabel.textContent = "Description: ";
    modalBody.appendChild(descriptionLabel);
    const descriptionTextarea = document.createElement("textarea");
    descriptionTextarea.type = "text";
    descriptionTextarea.value = board.description;
    descriptionTextarea.id = "boardDescription";
    modalBody.appendChild(descriptionTextarea);
    const updateBtn = document.createElement("button");
    updateBtn.textContent = "Edit";
    updateBtn.onclick = function() { updateBoard(id, board.projectId) };
    modalFooter.appendChild(updateBtn);
}

// Task items
async function addTaskItemModal(id) {
    modal.style.display = "block";
    modalHeader.innerHTML = "";
    modalBody.innerHTML = "";
    modalFooter.innerHTML = "";
    const title = document.createElement("h2");
    title.textContent = "Add Task";
    modalHeader.appendChild(title);
    const titleLabel = document.createElement("label");
    titleLabel.textContent = "Title: ";
    modalBody.appendChild(titleLabel);
    const titleInput = document.createElement("input");
    titleInput.type = "text";
    titleInput.id = "taskItemTitle";
    modalBody.appendChild(titleInput);
    const descriptionLabel = document.createElement("label");
    descriptionLabel.textContent = "Description: ";
    modalBody.appendChild(descriptionLabel);
    const descriptionTextarea = document.createElement("textarea");
    descriptionTextarea.type = "text";
    descriptionTextarea.id = "taskItemDescription";
    modalBody.appendChild(descriptionTextarea);
    const members = await getMembersWithTenantId();
    const memberSelect = document.createElement("select");
    memberSelect.name = "members";
    memberSelect.id = "membersForTask";
    modalBody.appendChild(memberSelect);
    members.forEach(member => {
        const option = document.createElement("option");
        option.value = member.id;
        option.textContent = member.userName;
        memberSelect.appendChild(option);
    });
    const addBtn = document.createElement("button");
    addBtn.textContent = "Add";
    addBtn.onclick = function() { addTaskItem(id) };
    modalFooter.appendChild(addBtn);
}

function updateTaskItemModalStyle(id, taskItem) {
    modal.style.display = "block";
    modalHeader.innerHTML = "";
    modalBody.innerHTML = "";
    modalFooter.innerHTML = "";
    const title = document.createElement("h2");
    title.textContent = "Edit task";
    modalHeader.appendChild(title);
    const titleLabel = document.createElement("label");
    titleLabel.textContent = "Title: ";
    modalBody.appendChild(titleLabel);
    const titleInput = document.createElement("input");
    titleInput.type = "text";
    titleInput.value = taskItem.title;
    titleInput.id = "taskItemTitle";
    modalBody.appendChild(titleInput);
    const descriptionLabel = document.createElement("label");
    descriptionLabel.textContent = "Description: ";
    modalBody.appendChild(descriptionLabel);
    const descriptionTextarea = document.createElement("textarea");
    descriptionTextarea.type = "text";
    descriptionTextarea.value = taskItem.description;
    descriptionTextarea.id = "taskItemDescription";
    modalBody.appendChild(descriptionTextarea);
    const statusLabel = document.createElement("label");
    statusLabel.textContent = "Status: ";
    modalBody.appendChild(statusLabel);
    const statusSelect = document.createElement("select");
    statusSelect.id = "taskItemStatus";
    modalBody.appendChild(statusSelect);
    const inProgressOption = document.createElement("option");
    inProgressOption.textContent = "In Progress";
    inProgressOption.value = "InProgress";
    statusSelect.appendChild(inProgressOption);
    const completedOption = document.createElement("option");
    completedOption.textContent = "Completed";
    completedOption.value = "Completed";
    statusSelect.appendChild(completedOption);
    const updateBtn = document.createElement("button");
    updateBtn.textContent = "Edit";
    updateBtn.onclick = function() { updateTaskItem(id) };
    modalFooter.appendChild(updateBtn);
}

window.onclick = function(event) {
    if(event.target == modal) {
        modal.style.display = "none";
    }
}