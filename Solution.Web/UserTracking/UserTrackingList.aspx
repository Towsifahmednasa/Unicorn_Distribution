<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="UserTrackingList.aspx.cs" Inherits="UserTracking_UserTrackingList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    
    <div id="coverScreen" class="divWaitingJquery" style="display:none">
        <img src="../images/Spinner.gif" style="width:180px" class="position-set" />
    </div>

    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> Location Tracking </div>

                <div class="ms-auto">
                    <div class="btn-group">

                    

                    </div>
                </div>
            </div>
            <!--end breadcrumb-->
            <div class="row">
                <div class="col">

                    <div class="card border-top border-0 border-4 border-success">
                        <div class="card-body">
                        
                          
                        <div class="row mt-1">
                            <div class="col-2">&nbsp;</div>
                            <div class="col-9">
                                <div class="form-group row">
                                    <label for="exampleInputUsername2" id="acttxt" class="col-sm-3 col-form-label"> Employee Name: </label>
                                    <div class="col-sm-5">
                                        <select id="ddlEmpName" name="UserSelect" class="form-select form-select-sm mb-3 mySelect2   mb-3">
                                        </select>


                                        <span id="v-acDate" class="invalid-tooltip fade hide" data-delay="2000">
                                        </span>
                                    </div>

                                    <span class="text-sm-left text-c-red">*</span>
                                </div>

                                <div style="padding:4px;"></div>
                                <div class="form-group row">
                                    <label for="exampleInputUsername2" id="acttxt" class="col-sm-3 col-form-label"> Date: </label>
                                    <div class="col-sm-5">

                                        <input type="text" id="dashboardDate2" autocomplete="off" placeholder="Select Date"  class="datepicker form-control form-control-sm">

                                    </div>


                                </div>
                            </div>




                        </div>

                        <div style="padding-top:16px;"></div>
                        <div class="row">
                            <div class="col-md-5">
                            </div>
                            <div class="col-md-4" style="align-content:center">
                                <button type="button" id="btnSave" class="btn btnMyDesignSearch btn-sm" onclick="GetUserLocation()">
                                    <i class="fa fa-search-plus"></i>&nbsp; Search
                                </button>
                                <button type="button" class="btn btnMyDesignReset btn-sm" onclick="ResetClick()"><i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset </button>
                            </div>
                        </div>
                        <div style="padding-top:4px;"></div>
                        <div id="mapid" style="width: 100%; height: 400px;z-index:1"></div>
                        <div style="padding-top:10px;"></div>
                        <div class="d-flex justify-content-end mb-2">
                            <button type="button" id="btnExport" class="btn btn-success btn-sm" onclick="exportTableToExcel()">
                                <i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp; Export to Excel
                            </button>
                        </div>
                        <div class="table-responsive">
                            <table class="table table-bordered table-striped table-sm mb-0" id="tblUserLocation">
                                <thead>
                                    <tr>
                                        <th style="width: 70px;">Sl</th>
                                        <th style="width: 180px;">Time</th>
                                        <th>Location</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="3" class="text-center">No data found</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div style="height:60px;"></div><%-- keeps last rows clear of the fixed page footer --%>
                    </div>
                </div>
            </div>
        </div>
    </div>

</div>


<link rel="stylesheet" href="https://unpkg.com/leaflet@1.7.1/dist/leaflet.css" integrity="sha512-xodZBNTC5n17Xt2atTPuE1HxjVMSvLVW9ocqUKLsCC5CXdbqCmblAshOMAS6/keqq/sMZMZ19scR4PsZChSR7A==" crossorigin="" />
<script src="https://unpkg.com/leaflet@1.7.1/dist/leaflet.js" integrity="sha512-XQoYMqMTK8LvdxXYG3nZ448hOEQiglfqkJs1NOQV44cWnUrBc8PkAOcXy20w0vlaXaVUearIOBhiXZ5V3ynxwA==" crossorigin=""></script>

    <script>

        var mymap;

        $('document').ready(function () {
            GetEmpList(0);

            $('.datepicker').pickadate({
                selectMonths: true,
                selectYears: true
            })

        });


        function GetEmpList(id) {
            _getEmployeeList_Active($('#ddlEmpName'), 'EmpInfoId', 'EmpName', id);
        }


        function ResetClick() {
            window.location.reload();
        }

        function GetUserLocation() {

            var empId = parseInt($('#ddlEmpName').val(), 10);
            // ISO date, so server parsing doesn't depend on pickadate's "26 September, 2026" format/culture
            var trackDate = $('#dashboardDate2').pickadate('picker').get('select', 'yyyy-mm-dd');

            if (!empId) { toastr.warning('Please select an employee'); return; }
            if (!trackDate) { toastr.warning('Please select a date'); return; }

            $.ajax({
                url: 'UserTrackingList.aspx/GetUserLocationTrackig',
                type: "POST", contentType: "application/json; charset=utf-8",
                async: true,
                data: JSON.stringify({ 'empId': empId, 'trackDate': trackDate }),

                dataType: 'json',
                beforeSend: function () {
                    $("#coverScreen").show();
                },
                success: function (data) {
                    var result = JSON.parse(data.d);
                    // Filter out empty or null locations
                    if (result) {
                        result = result.filter(function (item) {
                            return item.AddressName && item.AddressName.toString().trim() !== "" && item.AddressName.toString().trim().toLowerCase() !== "null";
                        });
                    }
                    result = SortLocationDataByTime(result);
                    BindLocationTable(result);
                    try { LoadMap(result); } catch (e) { console.error('LoadMap failed', e); }
                },
                error: function () {
                    toastr.error('Failed to load location data');
                },
                complete: function () {
                    $("#coverScreen").hide();
                }
            });
        }


        function LoadMap(result) {
            // rows with no GPS fix (NULL lat/long) blow up Leaflet with NaN; keep them in the table, skip them on the map
            result = (result || []).filter(function (r) {
                return isFinite(parseFloat(r.LatValue)) && isFinite(parseFloat(r.LongValue));
            });
            if (result.length === 0) {
                if (mymap != undefined && mymap != null) {
                    mymap.eachLayer(function (layer) {
                        if (!(layer instanceof L.TileLayer)) {
                            mymap.removeLayer(layer);
                        }
                    });
                }
                return;
            }

            if (mymap == undefined || mymap == null) {
                mymap = L.map('mapid').setView([23.810331, 90.412521], 10);
                L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                    maxZoom: 18,
                    attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">StreetMap</a> contributors, ' +
                        'Imagery © <a>Leaflet</a>',
                    tileSize: 512,
                    zoomOffset: -1
                }).addTo(mymap);
            } else {
                mymap.eachLayer(function (layer) {
                    if (!(layer instanceof L.TileLayer)) {
                        mymap.removeLayer(layer);
                    }
                });
            }

            var popup = L.popup();
            var polylinePoints = [];

            // for indicating start position
            var startObject = result[0];
            
            // Set map view to start position
            mymap.setView(lngLatToLatLng(parseFloat(startObject.LatValue), parseFloat(startObject.LongValue)), 13);

            L.marker(lngLatToLatLng(parseFloat(startObject.LatValue), parseFloat(startObject.LongValue))).addTo(mymap).bindPopup("Started at : " + startObject.Time + ": " + startObject.AddressName, { closeOnClick: false, autoClose: false }).openPopup();

            result.forEach(function (lngLat, i) {
                var latlngsd = lngLatToLatLng(parseFloat(lngLat.LatValue), parseFloat(lngLat.LongValue));
                polylinePoints[i] = latlngsd;

                if (result.length > 1) {
                    if (i > 0 && i < result.length - 1) {
                        L.marker(lngLatToLatLng(parseFloat(lngLat.LatValue), parseFloat(lngLat.LongValue))).addTo(mymap)
                            .bindPopup(lngLat.Time + " :" + lngLat.AddressName, { closeOnClick: false, autoClose: false }).openPopup();
                    }
                }
            });

            L.polyline(polylinePoints).addTo(mymap);
            if (result.length > 1) {
                var endObject = result[result.length - 1];
                L.marker(lngLatToLatLng(parseFloat(endObject.LatValue), parseFloat(endObject.LongValue))).addTo(mymap)
                    .bindPopup("Ended at : " + endObject.Time + " :" + endObject.AddressName, { closeOnClick: false, autoClose: false }).openPopup();
            }

        }

        function BindLocationTable(result) {
            var tbody = $('#tblUserLocation tbody');
            tbody.empty();

            if (!result || result.length === 0) {
                tbody.append('<tr><td colspan="3" class="text-center">No data found</td></tr>');
                return;
            }

            $.each(result, function (index, item) {
                var row = '<tr>' +
                    '<td>' + (index + 1) + '</td>' +
                    '<td>' + (item.Time || '') + '</td>' +
                    '<td>' + (item.AddressName || '') + '</td>' +
                    '</tr>';
                tbody.append(row);
            });
        }

        function SortLocationDataByTime(result) {
            if (!result || result.length === 0) {
                return [];
            }

            return result.sort(function (a, b) {
                return GetTimeSortValue(a.Time) - GetTimeSortValue(b.Time);
            });
        }

        function GetTimeSortValue(timeText) {
            if (!timeText) {
                return 0;
            }

            var text = timeText.toString().trim().toUpperCase().replace(/\s+/g, '');
            var match = text.match(/^(\d{1,2}):(\d{2}):?(\d{2})?(AM|PM)?$/);

            if (!match) {
                return 0;
            }

            var hour = parseInt(match[1], 10);
            var minute = parseInt(match[2], 10);
            var second = match[3] ? parseInt(match[3], 10) : 0;
            var meridiem = match[4];

            if (meridiem) {
                if (meridiem === 'PM' && hour !== 12) {
                    hour += 12;
                }
                if (meridiem === 'AM' && hour === 12) {
                    hour = 0;
                }
            }

            return (hour * 3600) + (minute * 60) + second;
        }

        function exportTableToExcel() {
            var tab_text = "<table border='2px'><tr bgcolor='#87AFC7'>";
            var textRange; var j = 0;
            var tab = document.getElementById('tblUserLocation'); // id of table

            for (j = 0 ; j < tab.rows.length ; j++) {
                tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
            }

            tab_text = tab_text + "</table>";
            tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if there's any links

            var file = new Blob([tab_text], { type: "application/vnd.ms-excel" });
            var url = URL.createObjectURL(file);
            var a = document.createElement('a');
            a.href = url;
            a.download = "Employee_Location_Report.xls";
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }

        function markerClick(e) {
            console.log(e);
        }

        function lngLatToLatLng(lat, lng) {
            return [lat, lng];
        }

    </script>
</asp:Content>
