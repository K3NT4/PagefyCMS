document.addEventListener("DOMContentLoaded", function () {

    // Slug-generator för sidor och artiklar
    const titleInput = document.querySelector(
        'input[name="NewPage.Title"], input[name="EditPage.Title"], input[name="NewArticle.Headline"], input[name="EditArticle.Headline"]'
    );

    const slugInput = document.querySelector(
        'input[name="NewPage.Slug"], input[name="EditPage.Slug"], input[name="NewArticle.Slug"], input[name="EditArticle.Slug"]'
    );

    if (titleInput && slugInput) {
        titleInput.addEventListener("input", function () {
            let slug = titleInput.value.toLowerCase()
                .replace(/\s+/g, '-')
                .replace(/[^a-z0-9\-]/g, '')
                .replace(/\-+/g, '-')
                .replace(/^-+|-+$/g, '');
            slugInput.value = slug;
        });
    }

    // Galleri-funktion med editor-stöd
    const gallery = document.getElementById("mediaGallery");
    const contentArea = document.getElementById("contentArea");
    const openGalleryBtn = document.getElementById("openGalleryBtn");
    const mediaModal = document.getElementById("mediaModal");

    if (openGalleryBtn && mediaModal && gallery && contentArea) {

        // Öppnar galleriet vid knapptryck (Bootstrap 5)
        openGalleryBtn.addEventListener("click", () => {
            const modal = new bootstrap.Modal(mediaModal);
            modal.show();
        });

        // Laddar bilder när modalen visas
        mediaModal.addEventListener("show.bs.modal", function () {
            gallery.innerHTML = "Laddar bilder...";
            fetch("/Admin/Media/ListImages")
                .then(res => res.json())
                .then(images => {
                    gallery.innerHTML = "";
                    images.forEach(img => {
                        const el = document.createElement("img");
                        el.src = img.url;
                        el.alt = img.altText || "";
                        el.classList.add("img-thumbnail", "m-2");
                        el.style.maxWidth = "150px";
                        el.style.cursor = "pointer";

                        el.addEventListener("click", () => {
                            const tag = `<img src="${img.url}" alt="${img.altText || ""}">`;

                            // TinyMCE
                            if (window.activeEditor && window.activeEditor.execCommand) {
                                window.activeEditor.execCommand('mceInsertContent', false, tag);
                            }
                            // Toast UI Editor
                            else if (window.activeEditor && window.activeEditor.insertText) {
                                window.activeEditor.insertText(tag);
                            }
                            // Fallback till vanlig textarea
                            else if (contentArea) {
                                insertAtCaret(contentArea, tag);
                            }

                            const modal = bootstrap.Modal.getInstance(mediaModal);
                            modal.hide();
                        });

                        gallery.appendChild(el);
                    });
                });
        });
    }

    // Infoga text vid markör i vanlig textarea
    function insertAtCaret(textarea, text) {
        const start = textarea.selectionStart;
        const end = textarea.selectionEnd;
        const before = textarea.value.substring(0, start);
        const after = textarea.value.substring(end);
        textarea.value = before + text + after;
        textarea.selectionStart = textarea.selectionEnd = start + text.length;
        textarea.focus();
    }
});
