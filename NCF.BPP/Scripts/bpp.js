function bpp_open_report(report) {
    var html = $("#embedReportHtml")[0].innerText;
    var w = window.open();
    w.document.write(html);
    w.init(report);
}

