document.addEventListener('DOMContentLoaded', () => {
  const faders = document.querySelectorAll('.fade-in');
  const observer = new IntersectionObserver(
    entries => entries.forEach(e => {
      if (e.isIntersecting) e.target.classList.add('appear');
    }), { threshold: 0.3 }
  );
  faders.forEach(el => observer.observe(el));
});


