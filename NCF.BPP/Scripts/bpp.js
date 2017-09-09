function bpp_open_report(report) {
    var html = $("#embedReportHtml")[0].innerText;
    var w = window.open();
   //$(w.document.body).html(html);
    //$(w.document).html(html);
    //w.document.documentElement.innerHTML = html;
    w.document.write(html);
    //w.accessToken = report.accessToken;
    //w.embedUrl = report.embedUrl;
    //w.embedReportId = report.reportId;
    w.init(report);
}

