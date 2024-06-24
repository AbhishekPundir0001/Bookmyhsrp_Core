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

//$(function () {
   
//    const calender = rome(inline_cal, {
//        min: '2024-05-10',
//        max: '2024-06-20',
//        "time": false,
//        dateValidator: rome.val.except(['2024-05-04', '2024-05-15']),
        
//    }); 
//    calender.on("back", AddHolidayClass)
//    calender.on("next", AddHolidayClass)
//    calender.on("ready", AddHolidayClass)
//    });

//function AddHolidayClass (d) {
  
//        var ele = $(".rd-day-body");

//        for (var i = 0; i < ele.length; i++) {
//            var eleVal = $(ele[i]).text();
//            if (eleVal === '15') {
//                $(ele[i]).addClass('holiday');
//            }
//        }
    
//}