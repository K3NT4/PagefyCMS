document.addEventListener("DOMContentLoaded", function () {

    // Automatisk slug-generator för sidor och artiklar
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

    // Gallerifunktion med editor-stöd
    const gallery = document.getElementById("mediaGallery");
    const contentArea = document.getElementById("contentArea");

    if (gallery && contentArea) {
        $('#mediaModal').on('show.bs.modal', function () {
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
                                contentArea.value += "\n" + tag;
                            }

                            $('#mediaModal').modal('hide');
                        });

                        gallery.appendChild(el);
                    });
                });
        });
    }
});
