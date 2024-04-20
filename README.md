# Unity Map Editor (With Gas)

## 目錄
- [介紹](#介紹)
- [包含部件](#包含部件)
    - [MapEditPackage.unitypackage](#MapEditPackage.unitypackage)
	- [其他](#其他)
- [基本配置](#基本配置)
- [操作與使用](#操作與使用)
	- [導出地圖](#導出地圖)
	- [導入地圖](#導入地圖)
- [詳細配置](#詳細配置)
	- [資料表及GAS設置](#資料表及GAS設置)
	- [Unity設置](#Unity設置)
- [重點欄位及方法](#重點欄位及方法)
	- [Map空物件](#Map空物件)
	- [MapManager.cs](#MapManager.cs)
	- [MapSheetEdit.gs](#MapSheetEdit.gs)
- [錯誤處理](#錯誤處理)

## 介紹

Unity Map Editor (With GAS) 是一款專為 Unity 編輯器環境設計的地圖編輯工具，提供了一個直觀的平台，方便使用者規劃和視覺化遊戲關卡地圖。此工具整合了 Google Apps Script (GAS) 和 Google 雲端試算表，允許使用者直接透過試算表更新和管理地圖數據，增強了工作流程的靈活性。
這份指南詳細介紹了如何使用 MapEditPackage.unitypackage 來導入和導出地圖數據，並提供了關於如何自定義功能的實用指引。這樣的設計使得地圖編輯更為高效，同時支持團隊協作和數據同步。

## 包含部件

### MapEditPackage.unitypackage
此包為開發必需的核心部件，請在發布頁面（release page）下載。
- Asset
  - Editor
    - **MapEditor**: 配合 MapManager 使用的 Unity Editor 腳本，允許自定義 MapManager 的界面和功能。
  - Scene
    - EditMapSampleScene: 為本專案提供的測試場景，包含用於演示的示範物件。
  - Script
    - **MapManager**: 專案的主要組件，負責地圖數據的匯入、匯出及解析，需掛載於 GameObject 上。
    - GASManager: 負責與 GAS 腳本進行網路通訊和處理回調的腳本，如選擇不使用 GAS 功能，需對此腳本進行相應修改。
    - MapItem: 定義地圖資料的結構，可根據需要進行自定義。
    - PointItem: 定義地圖點的資料結構，可根據需要進行自定義。
	
### 其他
除了核心部件外，如果需要使用自己的Google試算表進行地圖數據管理，還需準備：
- Google試算表：用於存儲和管理地圖數據。
- MapSheetEdit.gs：用於接收 GASManager 發起的網路通訊，並更新試算表或回傳試算表內容。

## 基本配置
- **匯入unitypackage**：
    - 請到發布頁面（release page）找到 `MapEditPackage.unitypackage`，並將其匯入至您的 Unity 專案中。匯入後，請檢查並確保所有的組件都完整無缺。
- **開啟測試場景**：
    - 打開 EditMapSampleScene，確保場景配置正確。正確的場景配置如下所示圖片，應該包括一個隱藏的圖片 Point_Default 和一個空物件 Map。
      
        ![Open_Sample_Scene](https://github.com/NaldCarol/MapPointProject/assets/27347610/3562c010-f781-4c07-8ae4-99382fb0a258)



## 操作與使用

### 導出地圖
1. 在瀏覽器中打開[試算表](https://docs.google.com/spreadsheets/d/19GZqwo9nSrj-0CVthICQJ8-KfkvzvJ9BLZK3jM_cAQc/edit#gid=1233568680)
2. 在Map空物件下新增並編輯地圖點。預設點物件為Point_Default，可複製後拖曳至Map空物件下使用。
3. 在 Hierarchy 視窗中選擇空物件Map，確保其已掛載 MapManager 組件。 

    ![Export03](https://github.com/NaldCarol/MapPointProject/assets/27347610/2a5f66a7-d59a-4b3b-87ac-c9ad940cdeb7)
5. 在MapManager的MapName欄位輸入地圖名稱(即試算表分頁名)。
6. 將圖片Point_Default拖曳至Point object here欄位。

    ![Export06](https://github.com/NaldCarol/MapPointProject/assets/27347610/8b6f4e0e-d597-4657-ae57-c8be8d455352)
7. 點擊 "Export Map to JSON" 按鈕。
8. 檢查 Console 視窗以確認導出成功，並檢查試算表是否有新增頁面。

    ![Export08](https://github.com/NaldCarol/MapPointProject/assets/27347610/1e818527-2a03-4377-9090-efaf4ce5f5ee)
### 導入地圖
1. 在瀏覽器中打開[試算表](https://docs.google.com/spreadsheets/d/19GZqwo9nSrj-0CVthICQJ8-KfkvzvJ9BLZK3jM_cAQc/edit#gid=1233568680)
2. 在 Hierarchy 視窗中選擇空物件Map，確保其已掛載 MapManager 組件。

    ![Export03](https://github.com/NaldCarol/MapPointProject/assets/27347610/2a5f66a7-d59a-4b3b-87ac-c9ad940cdeb7)
3. 將圖片Point_Default拖曳至Point object here欄位。

    ![Export06](https://github.com/NaldCarol/MapPointProject/assets/27347610/8b6f4e0e-d597-4657-ae57-c8be8d455352)
4. 在 MapEditor UI 中點擊 "Import from JSON" 按鈕。
5. 檢查 Console 視窗以確認導入成功，並檢查場景上地圖點是否正確顯示。

    ![Import06](https://github.com/NaldCarol/MapPointProject/assets/27347610/df75ab36-c80f-4808-81a1-a2392ce86cb8)


## 詳細配置

### 資料表及GAS設置
1. 在google雲端新增一個試算表。
2. 點擊「擴充功能」→「Apps Scripts」，新增一個GAS專案。

    ![New_GAS02](https://github.com/NaldCarol/MapPointProject/assets/27347610/90a02608-c36d-4a60-b969-3ed6462b70f6)
3. 複製MapSheetEdit.gs內容，並貼到編輯器中

    ![New_GAS03](https://github.com/NaldCarol/MapPointProject/assets/27347610/071856a7-89aa-4b06-ab4f-330ecf017401)
4. 複製試算表的ID（`https://docs.google.com/spreadsheets/d/**表id在此**/edit#gid=0`），並將其替換到MapSheetEdit.gs的第2行和第39行中，如下圖所示。

    ![New_GAS04](https://github.com/NaldCarol/MapPointProject/assets/27347610/e58abfc1-eb1b-4c17-8ddb-c27df1cf6b87)
5. (第一次新增)點擊「部署」→「新增部署作業」，並將設置調整成以下設置。

    ![New_GAS05](https://github.com/NaldCarol/MapPointProject/assets/27347610/760d1d26-da06-43e7-86c0-1a4dfb0808e4)

6. (第一次新增)點擊部署，然後點擊「授予存取權限」，完成接下來的授權流程。如果遇到下圖所示的情況，請點擊「Advanced」 → 「Go to\[你的專案名](unsafe)」，然後繼續完成後續操作。

    ![New_GAS06](https://github.com/NaldCarol/MapPointProject/assets/27347610/5fdad004-db6d-429f-ae2e-016f1f742f35)
7. (第一次新增)成功頁面出現後，請複製網頁應用程式網址以備用。
8. (後續修改)點擊「部署」 → 「管理部署作業」 → 「設定」/「編輯」 → 「版本」/「建立新版本」 → 「部署」，完成保存修改。

    ![New_GAS15](https://github.com/NaldCarol/MapPointProject/assets/27347610/81a95777-18f3-48d8-86b9-fc770f763899)

### Unity設置
1. 在場景中新增一個Canvas，並在其下新增一個空物件及一個Image，然後將Image的顯示關閉。
2. 打開**MapManager.cs**，將**googleScriptUrl**值改為網頁應用程式網址。網址可透過「部署」→「管理部署作業」頁面再次取得。

    ![New_UE02](https://github.com/NaldCarol/MapPointProject/assets/27347610/401f4c79-6bbb-4eba-8bc4-0ebf642c6a56)
3. 後續操作同[操作與使用](#操作與使用)一節。

## 重點欄位及方法

### Map空物件

![Export03](https://github.com/NaldCarol/MapPointProject/assets/27347610/2a5f66a7-d59a-4b3b-87ac-c9ad940cdeb7)
- Map name：地圖名稱欄位，同時也是試算表中的分頁名稱。
- Point Object Here：地圖點欄位，用於生成地圖點的基本物件，可以使用自定義的預製物件。
- ExportMapToJson：將場景中的地圖資料，匯出至雲端試算表的按鈕。
- ImportFromJson：從雲端試算表匯入地圖資料，並生成於場景上的按鈕。

### MapManager.cs
```
private static readonly string googleScriptUrl = "https://your/url/here";`
```
- 這是 GAS 的網址。如果你有自己的 GAS 配置，請修改此處的網址。

```
private void UploadData (string data)
{
    GASManager.Instance.DoPost(googleScriptUrl, data);
}
```
- 這個方法將地圖資料傳送給 GASManager 來處理。如果你不使用 GAS，請在這裡進行修改。

```
public void ImportMapFromJson (string mapName, GameObject pointPrefab)
{
    //Other method contents ...
    GASManager.Instance.DoGet(googleScriptUrl, mapName, OnJsonDataReceived);
}
```
- 這個方法將向 GASManager 發送請求。如果你不使用 GAS，請在這裡進行修改。

### MapSheetEdit.gs

```
function doGet(e) {
  //Other method contents ...
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
  //Other method contents ...
}
```
- 這段用於解析地圖點資料，並將其寫入到試算表中。如果地圖點的資料結構有所變化，請修改這部分內容。

```
function doPost(e) {
  //Other method contents ...
  
  var title = ["id","x","y"];
  
  //Other method contents ...

  for(let i = 0 ; i < length ; i++){
    var id = pointList[i].id;
    var x = pointList[i].x;
    var y = pointList[i].y;
    var data = [id,x,y];

    var range = sheet.getRange(i+2, 1, 1, data.length);
    range.setValues([data]);
  }
  
  //Other method contents ...
}
```
- 這段用於解析試算表中的地圖點資料，並回傳給客戶端。如果地圖點的資料結構有所變化，請修改這部分內容。

## 錯誤處理

>No default point prefab provided.
UnityEngine.Debug:LogWarning (object)

- 沒有可供生成地圖點的物件。請在 "Point Object Here" 欄位上放置地圖點的物件或預製物。

>Failed to deserialize JSON: No sheet called [Your map name]
UnityEngine.Debug:LogWarning (object)

- 試算表中沒有相應名稱的分頁，請檢查填在Map name欄位中填寫的名稱是否正確，或先編輯並新增地圖。

>HTTP/1.1 500 Internal Server Error
UnityEngine.Debug:LogWarning (object)

- googleScriptUrl為錯誤的網址，請檢查網址是否錯誤，如果確認網址正確，請重開unity專案再試。
