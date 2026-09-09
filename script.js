// ============ MOBILMENY ============
const menuToggle = document.getElementById("menuToggle");
const topnav = document.getElementById("topnav");

if (menuToggle && topnav) {
  menuToggle.addEventListener("click", () => {
    const isOpen = topnav.classList.toggle("is-open");
    menuToggle.setAttribute("aria-expanded", String(isOpen));
    document.body.classList.toggle("no-scroll", isOpen);
  });

  topnav.querySelectorAll("a").forEach((link) => {
    link.addEventListener("click", () => {
      topnav.classList.remove("is-open");
      menuToggle.setAttribute("aria-expanded", "false");
      document.body.classList.remove("no-scroll");
    });
  });
}

// ============ FAQ ACCORDION ============
document.querySelectorAll(".accordion").forEach((btn) => {
  btn.addEventListener("click", () => {
    const item = btn.closest(".faq");
    const isOpen = item.classList.contains("open");

    document.querySelectorAll(".faq.open").forEach((el) => {
      el.classList.remove("open");
      el.querySelector(".accordion").classList.remove("active");
    });

    if (!isOpen) {
      item.classList.add("open");
      btn.classList.add("active");
    }
  });
});

// ============ SCROLL REVEAL ============
const revealEls = document.querySelectorAll(".reveal");

if ("IntersectionObserver" in window && revealEls.length) {
  const observer = new IntersectionObserver(
    (entries) => {
      entries.forEach((entry) => {
        if (entry.isIntersecting) {
          entry.target.classList.add("is-visible");
          observer.unobserve(entry.target);
        }
      });
    },
    { threshold: 0.15, rootMargin: "0px 0px -40px 0px" }
  );
  revealEls.forEach((el) => observer.observe(el));
} else {
  revealEls.forEach((el) => el.classList.add("is-visible"));
}
