// Owl Carousel

$('.adds-carousel').owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    dots: false,
    autoplay: true,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 1
        },
        1000: {
            items: 1
        }
    }
})


$('.book-carousel').owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    dots: false,
    autoplay: true,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 1
        },
        1000: {
            items: 1
        }
    }
})
//Sticky Menu

$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('.mainmenu__wrap').addClass('sticky__header')
    } else {
        $('.mainmenu__wrap').removeClass('sticky__header')
    }
});

$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('.htc__header__top').addClass('sticky__mobile')
    } else {
        $('.htc__header__top').removeClass('sticky__mobile')
    }
});


//Calender

$(function () {

    rome(inline_cal, { time: false });

});