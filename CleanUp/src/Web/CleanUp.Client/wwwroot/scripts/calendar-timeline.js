function setupCalendarTimeline(events) {
    console.log("Entrato", events);
    var timetable = new Timetable();
    //timetable.setScope(7, 21); // optional, only whole hours between 0 and 23

    var operators = [...new Set(events.map(item => item.user.fullName))]; // [ 'A', 'B']
    timetable.addLocations(operators);

    events.forEach(item => {
        timetable.addEvent(item.event.classroomId, item.user.fullName, new Date(item.start), sumTimeToDate(new Date(item.start), item.duration));
    });
    //timetable.addEvent('Frankadelic', 'Nile', new Date(2015, 7, 17, 10, 45), new Date(2015, 7, 17, 12, 30));

    var renderer = new Timetable.Renderer(timetable);
    renderer.draw('.timetable'); // any css selector

    console.log("Uscito");
}

function sumTimeToDate(date, time) {
    var hours = parseInt(time.split(":")[0]);
    var minutes = parseInt(time.split(":")[1]);
    var seconds = parseInt(time.split(":")[2]);
    date.setTime(date.getTime() + (hours * 60 * 60 * 1000));
    date.setTime(date.getTime() + minutes * 60 * 1000);
    date.setTime(date.getTime() + seconds * 1000);
    return date;
}



//function setupCalendarTimeline() {
//    console.log("Ciccio", mobiscroll);
//    mobiscroll.setOptions({
//        locale: mobiscroll.localeIt,
//        theme: 'ios',
//        themeVariant: 'light'
//    });

//    var formatDate = mobiscroll.util.datetime.formatDate;
//    var startDate, endDate;

//    var myCalendar = mobiscroll.eventcalendar('#demo-custom-range-view', {
//        resources: [{
//            id: 1,
//            name: 'Resource A',
//            color: '#e20000'
//        }, {
//            id: 2,
//            name: 'Resource B',
//            color: '#76e083'
//        }, {
//            id: 3,
//            name: 'Resource C',
//            color: '#4981d6'
//        }, {
//            id: 4,
//            name: 'Resource D',
//            color: '#e25dd2'
//        }, {
//            id: 5,
//            name: 'Resource E',
//            color: '#1dab2f'
//        }, {
//            id: 6,
//            name: 'Resource F',
//            color: '#d6d145'
//        }],
//        view: {
//            timeline: {
//                type: 'day',
//                startTime: '08:00',
//                endTime: '19:00',
//                size: 5,
//                eventList: true,
//                timeCellStep: 50,
//                timeLabelStep: 60
//            }
//        },
//        renderHeader: function () {
//            return '<div id="custom-date-range"><button mbsc-button data-variant="flat" class="mbsc-calendar-button">' +
//                '<span id="custom-date-range-text" class="mbsc-calendar-title">Button' +
//                '</span></button></div>' +
//                '<div class="md-custom-range-view-controls">' +
//                '<div mbsc-calendar-prev></div>' +
//                '<div mbsc-calendar-today></div>' +
//                '<div mbsc-calendar-next></div>' +
//                '</div>';
//        },
//        onPageLoaded: function (args) {
//            console.log("OnPageLoaded");
//            startDate = args.firstDay;
//            end = args.lastDay;
//            endDate = new Date(end.getFullYear(), end.getMonth(), end.getDate() - 1, 0);
//            // set button text
//            rangeButton.innerText = getFormattedRange(startDate, endDate);
//            // set range value
//            myRange.setVal([startDate, endDate]);
//        }
//    });

//    var myRange = mobiscroll.datepicker('#custom-date-range', {
//        select: 'range',
//        display: 'anchored',
//        showOverlay: false,
//        touchUi: true,
//        buttons: [],
//        onClose: function (args, inst) {
//            var date = inst.getVal();
//            if (date[0] && date[1]) {
//                if (date[0].getTime() !== startDate.getTime()) {
//                    // navigate the calendar
//                    myCalendar.navigate(date[0]);
//                }
//                startDate = date[0];
//                endDate = date[1];
//                // set calendar view
//                myCalendar.setOptions({
//                    refDate: startDate,
//                    view: {
//                        timeline: {
//                            type: 'day',
//                            size: getNrDays(startDate, endDate),
//                            eventList: true
//                        }
//                    }
//                });
//            } else {
//                myRange.setVal([startDate, endDate])
//            }
//        }
//    });

//    var rangeButton = document.getElementById('custom-date-range-text');

//    // returns the formatted date
//    function getFormattedRange(start, end) {
//        return formatDate('MMM D, YYYY', new Date(start)) + (end && getNrDays(start, end) > 1 ? (' - ' + formatDate('MMM D, YYYY', new Date(end))) : '');
//    }

//    // returns the number of days between two dates
//    function getNrDays(start, end) {
//        return Math.round(Math.abs((end.setHours(0) - start.setHours(0)) / (24 * 60 * 60 * 1000))) + 1;
//    }

//    mobiscroll.util.http.getJson('https://trial.mobiscroll.com/timeline-events/?vers=5', function (events) {
//        myCalendar.setEvents(events);
//    }, 'jsonp');
//}