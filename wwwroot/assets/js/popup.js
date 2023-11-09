$(function() {
  
    // contact form animations
    $('#contact').click(function() {
      $('#contactForm3').fadeToggle();
    })
    $(document).mouseup(function (e) {
      var container = $("#contactForm3");
  
        var scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;

        if (!container.is(e.target) // if the target of the click isn't the container...
            && container.has(e.target).length === 0 // ... nor a descendant of the container
            && !(e.clientX >= window.innerWidth - scrollbarWidth)) // ... nor the scrollbar
        {
            container.fadeOut();
        }

    });
    
  });
  
  $(function() {
    
    // contact form animations
    $('#contact2').click(function() {
      $('#contactForm2').fadeToggle();
    })
    $(document).mouseup(function (e) {
      var container = $("#contactForm2");
  
        var scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;

        if (!container.is(e.target) // if the target of the click isn't the container...
            && container.has(e.target).length === 0 // ... nor a descendant of the container
            && !(e.clientX >= window.innerWidth - scrollbarWidth)) // ... nor the scrollbar
        {
            container.fadeOut();
        }
    });
    
  });

  $(function() {
    
    // contact form animations
    $('#contactAddProduct').click(function() {
      $('#contactFormAddProduct').fadeToggle();
    })
    $(document).mouseup(function (e) {
      var container = $("#contactFormAddProduct");
  
        var scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;

        if (!container.is(e.target) // if the target of the click isn't the container...
            && container.has(e.target).length === 0 // ... nor a descendant of the container
            && !(e.clientX >= window.innerWidth - scrollbarWidth)) // ... nor the scrollbar
        {
            container.fadeOut();
        }
    });
    
  });

  $(function() {
    
    // contact form animations
    $('#contactAddCategory').click(function() {
      $('#contactFormAddCategory').fadeToggle();
    })
    $(document).mouseup(function (e) {
      var container = $("#contactFormAddCategory");
  
        var scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;

        if (!container.is(e.target) // if the target of the click isn't the container...
            && container.has(e.target).length === 0 // ... nor a descendant of the container
            && !(e.clientX >= window.innerWidth - scrollbarWidth)) // ... nor the scrollbar
        {
            container.fadeOut();
        }
    });
    
  });

  $(function() {
    
    // contact form animations
    $('#contactAddBrand').click(function() {
      $('#contactFormAddBrand').fadeToggle();
    })
    $(document).mouseup(function (e) {
      var container = $("#contactFormAddBrand");
  
        var scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;

        if (!container.is(e.target) // if the target of the click isn't the container...
            && container.has(e.target).length === 0 // ... nor a descendant of the container
            && !(e.clientX >= window.innerWidth - scrollbarWidth)) // ... nor the scrollbar
        {
            container.fadeOut();
        }
    });
    
  });

  $(function() {
    
    // contact form animations
    $('#contactProductInfo').click(function() {
      $('#contactFormProductInfoEdit').fadeToggle();
    })
    $(document).mouseup(function (e) {
      var container = $("#contactFormProductInfoEdit");
  
        var scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;

        if (!container.is(e.target) // if the target of the click isn't the container...
            && container.has(e.target).length === 0 // ... nor a descendant of the container
            && !(e.clientX >= window.innerWidth - scrollbarWidth)) // ... nor the scrollbar
        {
            container.fadeOut();
        }

    });
    
  });

  $(function() {
    
    // contact form animations
    //$('.contactBrandEdit').click(function() {
    //  $('#contactFormBrandEdit').fadeToggle();
    //})
    $(document).mouseup(function (e) {
      var container = $("#contactFormBrandEdit");
  
      if (!container.is(e.target) // if the target of the click isn't the container...
          && container.has(e.target).length === 0) // ... nor a descendant of the container
      {
          container.fadeOut();
      }
    });
    
  });

  $(function() {
    
    // contact form animations
    //$('#contactCategoryEdit').click(function() {
      //$('#contactFormEditCategory').fadeToggle();
    //})
    $(document).mouseup(function (e) {
      var container = $("#contactFormEditCategory");
  
      if (!container.is(e.target) // if the target of the click isn't the container...
          && container.has(e.target).length === 0) // ... nor a descendant of the container
      {
          container.fadeOut();
      }
    });
    
  });