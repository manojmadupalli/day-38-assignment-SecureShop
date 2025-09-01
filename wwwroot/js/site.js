
(function (){
    // Simple client-side enhancement: dismiss alerts automatically after 5s
    setTimeout(() => {
        document.querySelectorAll('.alert').forEach(a => {
            if (a.classList.contains('show')) {
                const bsAlert = bootstrap.Alert.getOrCreateInstance(a);
                bsAlert.close();
            }
        });
    }, 5000);
})();
