// ArgusCord Landing Page JS
document.addEventListener("DOMContentLoaded", () => {
    const header = document.querySelector(".header");
    const langToggle = document.getElementById("lang-toggle");

    // Language Toggle logic
    const setLanguage = (lang) => {
        document.documentElement.setAttribute("lang", lang);
        if (lang === "ar") {
            document.documentElement.setAttribute("dir", "rtl");
        } else {
            document.documentElement.setAttribute("dir", "ltr");
        }
        localStorage.setItem("arguscord-lang", lang);
    };

    // Load saved language or default to English
    const savedLang = localStorage.getItem("arguscord-lang") || "en";
    setLanguage(savedLang);

    if (langToggle) {
        langToggle.addEventListener("click", () => {
            const currentLang = document.documentElement.getAttribute("lang");
            const newLang = currentLang === "en" ? "ar" : "en";
            setLanguage(newLang);
        });
    }

    // Dynamic Header Background on Scroll
    window.addEventListener("scroll", () => {
        if (window.scrollY > 50) {
            header.style.backgroundColor = "rgba(6, 4, 11, 0.85)";
            header.style.boxShadow = "0 10px 30px rgba(0,0,0,0.3)";
        } else {
            header.style.backgroundColor = "rgba(6, 4, 11, 0.7)";
            header.style.boxShadow = "none";
        }
    });

    // Smooth Scroll for local links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener("click", function(e) {
            e.preventDefault();
            const targetId = this.getAttribute("href");
            const targetElement = document.querySelector(targetId);

            if (targetElement) {
                targetElement.scrollIntoView({
                    behavior: "smooth",
                    block: "start"
                });
            }
        });
    });

    console.log("ArgusCord Landing Page initialized.");
});
