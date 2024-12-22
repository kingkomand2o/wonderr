document.addEventListener('DOMContentLoaded', function() {
    // Header scroll effect
    const header = document.querySelector('header');
    
    window.addEventListener('scroll', () => {
        if (window.scrollY > 50) {
            header.classList.add('scrolled');
        } else {
            header.classList.remove('scrolled');
        }
    });

    // Hamburger menu
    const hamburger = document.getElementById('hamburger');
    const sidebar = document.getElementById('sidebar');

    hamburger.addEventListener('click', (e) => {
        e.stopPropagation();
        hamburger.classList.toggle('active');
        sidebar.classList.toggle('open');
    });

    document.addEventListener('click', (e) => {
        if (!sidebar.contains(e.target) && sidebar.classList.contains('open')) {
            hamburger.classList.remove('active');
            sidebar.classList.remove('open');
        }
    });

    sidebar.addEventListener('click', (e) => {
        e.stopPropagation();
    });

    // Scroll Animations
    const animateOnScroll = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate');
            }
        });
    }, {
        threshold: 0.1
    });

    // Elements to animate
    const sections = [
        '.hero-content',
        '.developers-section .card',
        '.projects-section .card',
        '.success-numbers .stat',
        '.newsletter-section',
        '.footer-content > div'
    ];

    sections.forEach(selector => {
        const elements = document.querySelectorAll(selector);
        elements.forEach(element => {
            element.classList.add('animate-on-scroll');
            animateOnScroll.observe(element);
        });
    });

    // Counter animation for success numbers
    const animateCounter = (element, target) => {
        let current = 0;
        const increment = target / 50; // Adjust speed here
        const timer = setInterval(() => {
            current += increment;
            if (current >= target) {
                element.textContent = target + '+';
                clearInterval(timer);
            } else {
                element.textContent = Math.floor(current) + '+';
            }
        }, 30);
    };

    const counterObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const target = parseInt(entry.target.getAttribute('data-target'));
                animateCounter(entry.target, target);
                counterObserver.unobserve(entry.target);
            }
        });
    }, { threshold: 0.5 });

    // Initialize counters
    document.querySelectorAll('.stat h3').forEach(counter => {
        const value = parseInt(counter.textContent);
        counter.setAttribute('data-target', value);
        counter.textContent = '0+';
        counterObserver.observe(counter);
    });
});