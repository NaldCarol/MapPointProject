function doGet(e) {
  var app = SpreadsheetApp.openById("19GZqwo9nSrj-0CVthICQJ8-KfkvzvJ9BLZK3jM_cAQc");
  var name = e.parameter.name;  
  var sheet = app.getSheetByName(name);

  if (sheet == null) {
    return ContentService.createTextOutput("No sheet called "+name);
  }

  var dataRange = sheet.getDataRange();
  var value = dataRange.getValues();
  var data = {
  "name": name,
  "pointList": []
  };
  
  for(let i = 1 ; i < value.length ; i++){
    var id = value[i][0];
    var x = value[i][1];
    var y = value[i][2];
    var point = {
      "id":id,
      "x":x,
      "y":y
    };

    data.pointList.push(point);
  }

  var result = JSON.stringify(data);
  return ContentService.createTextOutput(result);
}

function doPost(e) {
  var dataString = e.postData.contents;
  var dataObject = JSON.parse(dataString);
  
  var name = dataObject.name;
  var app = SpreadsheetApp.openById("19GZqwo9nSrj-0CVthICQJ8-KfkvzvJ9BLZK3jM_cAQc");
  var sheet = app.getSheetByName(name);

  if (sheet == null) {  
    sheet = app.insertSheet(name);
  } else {
    sheet.clear();
  }
  
  var title = ["id","x","y"];
  sheet.appendRow(title);
  
  var pointList = dataObject.pointList;
  var length = pointList.length;
  for(let i = 0 ; i < length ; i++){
    var id = pointList[i].id;
    var x = pointList[i].x;
    var y = pointList[i].y;
    var data = [id,x,y];

    var range = sheet.getRange(i+2, 1, 1, data.length);
    range.setValues([data]);
  }
  
  return ContentService.createTextOutput("Sheet "+name+" successfully refresh.");
}