document.addEventListener("DOMContentLoaded", function () {
  // Initialize all countdowns
  document.querySelectorAll("[id^=countdown-]").forEach(function (countdownElement) {
    var endDate = new Date(countdownElement.getAttribute("data-enddate"));

    function updateCountdown() {
      var now = new Date();
      var timeDiff = endDate - now;

      if (timeDiff > 0) {
        var days = Math.floor(timeDiff / (1000 * 60 * 60 * 24));
        var hours = Math.floor((timeDiff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((timeDiff % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((timeDiff % (1000 * 60)) / 1000);

        countdownElement.textContent = `${days}d ${hours}h ${minutes}m ${seconds}s`;
      } else {
        countdownElement.textContent = "Auction ended";
      }
    }

    // Initial update and set interval to update every second
    updateCountdown();
    setInterval(updateCountdown, 1000);
  });
});

const myCarouselElement = document.querySelector('#myCarousel')

const carousel = new bootstrap.Carousel(myCarouselElement, {
  interval: 3000,
  touch: false
})