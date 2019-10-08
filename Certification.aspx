<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Certification.aspx.cs" Inherits="Certification" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <link type="text/css" href="js/searchabledropdown/3.3.7-css-bootstrap.min.css" rel="stylesheet" />
    <link type="text/css" href="js/searchabledropdown/1.12.2-css-select.min.css" rel="stylesheet" />
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <style>
        .dropdownCustom {
            width: 280px !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#<%= txtRefrenceDate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtDateOfReceipt.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtDateOfCalib.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        function onlyNos(e, t) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            }
            catch (err) {
                alert(err.Description);
            }
        }
        function isNumberKey(txt, evt) {

            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode == 46) {
                //Check if the text already contains the . character
                if (txt.value.indexOf('.') === -1) {
                    return true;
                } else {
                    return false;
                }
            } else {
                if (charCode > 31
                     && (charCode < 48 || charCode > 57))
                    return false;
            }
            return true;
        }
        function CalculateValue() {
            //var Number1 = $("#wuc1_txtNumber1").val();
            //var Number2 = $("#wuc1_txtNumber2").val();

            //var result = parseInt(Number1) + parseInt(Number2);

            //$("#wuc1_txtResult").val(result);
        }

        $(document).ready(function () {
            //MICROMETER
            $(".calculation").blur(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdCalibResult_txtR1_0
                var t1 = document.getElementById('MainContent_grdCalibResult_txtR1_' + spId).value;
                var t2 = document.getElementById('MainContent_grdCalibResult_txtR2_' + spId).value;
                var t3 = document.getElementById('MainContent_grdCalibResult_txtR3_' + spId).value;

                var t4 = document.getElementById('MainContent_grdCalibResult_txtObserved_' + spId).value;
                var t5 = document.getElementById('MainContent_grdCalibResult_txtError_' + spId).value;

                var setDefault = "0";
                var r1 = 0;
                var r2 = 0;
                var r3 = 0;
                var observed = 0;
                var error = 0;

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                if (t3 != "") {
                    r3 = parseFloat(t3);
                }
                if (t4 != "") {
                    observed = parseFloat(t4);
                }
                if (t5 != "") {
                    error = parseFloat(t5);
                }
                var result = (parseFloat(r1) + parseFloat(r2) + parseFloat(r3));
                var allresult = result / 3;

                document.getElementById('MainContent_grdCalibResult_txtObserved_' + spId).value = parseFloat(allresult).toFixed(4);

                var nominalSize = document.getElementById('MainContent_grdCalibResult_txtnominalsize_' + spId).value;
                var errorVal = parseFloat(nominalSize) - allresult;
                document.getElementById('MainContent_grdCalibResult_txtError_' + spId).value = parseFloat(errorVal).toFixed(4);
            });

            $('.calculation').keyup(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdCalibResult_txtR1_0
                var t1 = document.getElementById('MainContent_grdCalibResult_txtR1_' + spId).value;
                var t2 = document.getElementById('MainContent_grdCalibResult_txtR2_' + spId).value;
                var t3 = document.getElementById('MainContent_grdCalibResult_txtR3_' + spId).value;

                var t4 = document.getElementById('MainContent_grdCalibResult_txtObserved_' + spId).value;
                var t5 = document.getElementById('MainContent_grdCalibResult_txtError_' + spId).value;

                var setDefault = "0";
                var r1 = 0;
                var r2 = 0;
                var r3 = 0;
                var observed = 0;
                var error = 0;

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                if (t3 != "") {
                    r3 = parseFloat(t3);
                }
                if (t4 != "") {
                    observed = parseFloat(t4);
                }
                if (t5 != "") {
                    error = parseFloat(t5);
                }
                var result = (parseFloat(r1) + parseFloat(r2) + parseFloat(r3));
                var allresult = result / 3;

                document.getElementById('MainContent_grdCalibResult_txtObserved_' + spId).value = parseFloat(allresult).toFixed(4);

                var nominalSize = document.getElementById('MainContent_grdCalibResult_txtnominalsize_' + spId).value;
                var errorVal = parseFloat(nominalSize) - allresult;
                document.getElementById('MainContent_grdCalibResult_txtError_' + spId).value = parseFloat(errorVal).toFixed(4);
                //CalculateValue();

            });
            //vernierintcalculation
            $('.vernierintcalculation').blur(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                // MainContent_grdVernier_    txtexternalErrorTopR1_
                var tinEtopR1 = document.getElementById('MainContent_grdVernier_txtInternalErrorTopR1_' + spId).value;
                var tinEbottomR1 = document.getElementById('MainContent_grdVernier_txtInternalErrorBottomR1_' + spId).value;
                var tinEtopR2 = document.getElementById('MainContent_grdVernier_txtInternalErrorTopR2_' + spId).value;
                var tinEbottomR2 = document.getElementById('MainContent_grdVernier_txtInternalErrorBottomR2_' + spId).value;
                var tinEAvgtop = document.getElementById('MainContent_grdVernier_txtInternalAvgErrorTop_' + spId).value;
                var tinEAvgbottom = document.getElementById('MainContent_grdVernier_txtInternalAvgErrorBottom_' + spId).value;
                var tinCalErrortop = document.getElementById('MainContent_grdVernier_hfIntError1Top_' + spId).value;
                var tinCalErrorbottom = document.getElementById('MainContent_grdVernier_hfIntError1Bottom_' + spId).value;

                var topR1 = 0;
                var bottomR1 = 0;
                var topR2 = 0;
                var bottomR2 = 0;
                var extAvgtop = 0;
                var extAvgbottom = 0;
                var errortop = 0;
                var errorbottom = 0;
                if (tinEtopR1 != "") {
                    topR1 = parseFloat(tinEtopR1);
                }
                if (tinEbottomR1 != "") {
                    bottomR1 = parseFloat(tinEbottomR1);
                }
                if (tinEtopR2 != "") {
                    topR2 = parseFloat(tinEtopR2);

                }
                if (tinEbottomR2 != "") {
                    bottomR2 = parseFloat(tinEbottomR2);
                }
                //Calculation Part
                var topAvgVal = topR1 + topR2;
                topAvgVal = topAvgVal / 2;
                var bottomAvgVal = bottomR1 + bottomR2;
                bottomAvgVal = bottomAvgVal / 2;
                intAvgtop = parseFloat(topAvgVal).toFixed(4);
                intAvgbottom = parseFloat(bottomAvgVal).toFixed(4);

                document.getElementById('MainContent_grdVernier_txtInternalAvgErrorTop_' + spId).value = parseFloat(intAvgtop);
                document.getElementById('MainContent_grdVernier_txtInternalAvgErrorBottom_' + spId).value = parseFloat(intAvgbottom);

                var nominalSize = document.getElementById('MainContent_grdVernier_txtnominalsizeVernier_' + spId).value;

                errortop = parseFloat(nominalSize) - parseFloat(intAvgtop);
                errorbottom = parseFloat(nominalSize) - parseFloat(intAvgbottom);

                document.getElementById('MainContent_grdVernier_hfIntError1Top_' + spId).value = parseFloat(errortop).toFixed(4);
                document.getElementById('MainContent_grdVernier_hfIntError1Bottom_' + spId).value = parseFloat(errorbottom).toFixed(4);
            });
            $('.vernierintcalculation').keyup(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                // MainContent_grdVernier_    txtexternalErrorTopR1_
                var tinEtopR1 = document.getElementById('MainContent_grdVernier_txtInternalErrorTopR1_' + spId).value;
                var tinEbottomR1 = document.getElementById('MainContent_grdVernier_txtInternalErrorBottomR1_' + spId).value;
                var tinEtopR2 = document.getElementById('MainContent_grdVernier_txtInternalErrorTopR2_' + spId).value;
                var tinEbottomR2 = document.getElementById('MainContent_grdVernier_txtInternalErrorBottomR2_' + spId).value;
                var tinEAvgtop = document.getElementById('MainContent_grdVernier_txtInternalAvgErrorTop_' + spId).value;
                var tinEAvgbottom = document.getElementById('MainContent_grdVernier_txtInternalAvgErrorBottom_' + spId).value;
                var tinCalErrortop = document.getElementById('MainContent_grdVernier_hfIntError1Top_' + spId).value;
                var tinCalErrorbottom = document.getElementById('MainContent_grdVernier_hfIntError1Bottom_' + spId).value;

                var topR1 = 0;
                var bottomR1 = 0;
                var topR2 = 0;
                var bottomR2 = 0;
                var extAvgtop = 0;
                var extAvgbottom = 0;
                var errortop = 0;
                var errorbottom = 0;
                if (tinEtopR1 != "") {
                    topR1 = parseFloat(tinEtopR1);
                }
                if (tinEbottomR1 != "") {
                    bottomR1 = parseFloat(tinEbottomR1);
                }
                if (tinEtopR2 != "") {
                    topR2 = parseFloat(tinEtopR2);

                }
                if (tinEbottomR2 != "") {
                    bottomR2 = parseFloat(tinEbottomR2);
                }
                //Calculation Part
                var topAvgVal = topR1 + topR2;
                topAvgVal = topAvgVal / 2;
                var bottomAvgVal = bottomR1 + bottomR2;
                bottomAvgVal = bottomAvgVal / 2;
                intAvgtop = parseFloat(topAvgVal).toFixed(4);
                intAvgbottom = parseFloat(bottomAvgVal).toFixed(4);

                document.getElementById('MainContent_grdVernier_txtInternalAvgErrorTop_' + spId).value = parseFloat(intAvgtop);
                document.getElementById('MainContent_grdVernier_txtInternalAvgErrorBottom_' + spId).value = parseFloat(intAvgbottom);

                var nominalSize = document.getElementById('MainContent_grdVernier_txtnominalsizeVernier_' + spId).value;

                errortop = parseFloat(nominalSize) - parseFloat(intAvgtop);
                errorbottom = parseFloat(nominalSize) - parseFloat(intAvgbottom);

                document.getElementById('MainContent_grdVernier_hfIntError1Top_' + spId).value = parseFloat(errortop).toFixed(4);
                document.getElementById('MainContent_grdVernier_hfIntError1Bottom_' + spId).value = parseFloat(errorbottom).toFixed(4);
            });
            //Lever Bore
            $('.leverborecalculation').blur(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdLeverBore_txtR1_0

                var t1 = document.getElementById('MainContent_grdLeverBore_txtUpErrorpR1_' + spId).value;
                var t2 = document.getElementById('MainContent_grdLeverBore_txtUpErrorpR2_' + spId).value;
                var t3 = document.getElementById('MainContent_grdLeverBore_txtDownErrorR1_' + spId).value;
                var t4 = document.getElementById('MainContent_grdLeverBore_txtDownErrorR2_' + spId).value;

                var setDefault = "0";
                var r1 = 0;
                var r2 = 0;
                var r3 = 0;
                var r4 = 0;
                var upReading = 0;
                var downReading = 0;
                var uperror = 0;
                var downerror = 0;

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                if (t3 != "") {
                    r3 = parseFloat(t3);
                }
                if (t4 != "") {
                    r4 = parseFloat(t4);
                }

                var resultUp = (parseFloat(r1) + parseFloat(r3));
                resultUp = resultUp / 2;
                var resultDown = (parseFloat(r2) + parseFloat(r4));
                resultDown = resultDown / 2;
                document.getElementById('MainContent_grdLeverBore_txtUpward_' + spId).value = parseFloat(resultUp).toFixed(4);
                document.getElementById('MainContent_grdLeverBore_txtDownward_' + spId).value = parseFloat(resultDown).toFixed(4);
                var nominalSize = document.getElementById('MainContent_grdLeverBore_txtnominalsizeLeverBore_' + spId).value;
                uperror = nominalSize - resultUp;
                downerror = nominalSize - resultDown;
                document.getElementById('MainContent_grdLeverBore_hfUpError_' + spId).value = parseFloat(uperror).toFixed(4);
                document.getElementById('MainContent_grdLeverBore_hfDownError_' + spId).value = parseFloat(downerror).toFixed(4);
            });
            $('.leverborecalculation').keyup(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdLeverBore_txtR1_0

                var t1 = document.getElementById('MainContent_grdLeverBore_txtUpErrorpR1_' + spId).value;
                var t2 = document.getElementById('MainContent_grdLeverBore_txtUpErrorpR2_' + spId).value;
                var t3 = document.getElementById('MainContent_grdLeverBore_txtDownErrorR1_' + spId).value;
                var t4 = document.getElementById('MainContent_grdLeverBore_txtDownErrorR2_' + spId).value;

                var setDefault = "0";
                var r1 = 0;
                var r2 = 0;
                var r3 = 0;
                var r4 = 0;
                var upReading = 0;
                var downReading = 0;
                var uperror = 0;
                var downerror = 0;

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                if (t3 != "") {
                    r3 = parseFloat(t3);
                }
                if (t4 != "") {
                    r4 = parseFloat(t4);
                }

                var resultUp = (parseFloat(r1) + parseFloat(r3));
                resultUp = resultUp / 2;
                var resultDown = (parseFloat(r2) + parseFloat(r4));
                resultDown = resultDown / 2;
                document.getElementById('MainContent_grdLeverBore_txtUpward_' + spId).value = parseFloat(resultUp).toFixed(4);
                document.getElementById('MainContent_grdLeverBore_txtDownward_' + spId).value = parseFloat(resultDown).toFixed(4);
                var nominalSize = document.getElementById('MainContent_grdLeverBore_txtnominalsizeLeverBore_' + spId).value;
                uperror = nominalSize - resultUp;
                downerror = nominalSize - resultDown;
                document.getElementById('MainContent_grdLeverBore_hfUpError_' + spId).value = parseFloat(uperror).toFixed(4);
                document.getElementById('MainContent_grdLeverBore_hfDownError_' + spId).value = parseFloat(downerror).toFixed(4);
            });
            $('.feelercalculation').keyup(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdFeeler_txtnominalsizeFeeler_0

                var t1 = document.getElementById('MainContent_grdFeeler_txtnominalsizeFeeler_' + spId).value;
                var t2 = document.getElementById('MainContent_grdFeeler_txtObserved_' + spId).value;
                var t3 = document.getElementById('MainContent_grdFeeler_txtvariation_' + spId).value;

                var setDefault = "0";
                var r1 = 0;
                var r2 = 0;
                var r3 = 0;

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                if (t3 != "") {
                    r3 = parseFloat(t3);
                }

                var resultVariation = (parseFloat(r1) - parseFloat(r2));
                document.getElementById('MainContent_grdFeeler_txtvariation_' + spId).value = parseFloat(resultVariation).toFixed(4);
            });
            //Pressure Gauge
            $('.forwardPressurecal').blur(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdPressureFowrding
                var tnominal = document.getElementById('MainContent_grdPressureFowrding_txtnominalsizeForward_' + spId).value;
                var t1 = document.getElementById('MainContent_grdPressureFowrding_txtForwardR1_' + spId).value;
                var t2 = document.getElementById('MainContent_grdPressureFowrding_txtForwardR2_' + spId).value;
                var t3 = document.getElementById('MainContent_grdPressureFowrding_txtForwardR3_' + spId).value;
                var nominal = 0;
                var r1 = 0;
                var r2 = 0;
                var r3 = 0;

                if (tnominal != "") {
                    nominal = parseFloat(tnominal);
                }

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                if (t3 != "") {
                    r3 = parseFloat(t3);
                }


                var resultSum = (parseFloat(r1) + parseFloat(r2) + parseFloat(r3));
                var meanReading = resultSum / 3;
                document.getElementById('MainContent_grdPressureFowrding_txtForwardMeanReading_' + spId).value = parseFloat(meanReading).toFixed(4);
                var error = nominal - meanReading;
                document.getElementById('MainContent_grdPressureFowrding_txtForwardError_' + spId).value = parseFloat(error).toFixed(4);
            });
            $('.forwardPressurecal').keyup(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdPressureFowrding
                var tnominal = document.getElementById('MainContent_grdPressureFowrding_txtnominalsizeForward_' + spId).value;
                var t1 = document.getElementById('MainContent_grdPressureFowrding_txtForwardR1_' + spId).value;
                var t2 = document.getElementById('MainContent_grdPressureFowrding_txtForwardR2_' + spId).value;
                var t3 = document.getElementById('MainContent_grdPressureFowrding_txtForwardR3_' + spId).value;
                var nominal = 0;
                var r1 = 0;
                var r2 = 0;
                var r3 = 0;

                if (tnominal != "") {
                    nominal = parseFloat(tnominal);
                }

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                if (t3 != "") {
                    r3 = parseFloat(t3);
                }


                var resultSum = (parseFloat(r1) + parseFloat(r2) + parseFloat(r3));
                var meanReading = resultSum / 3;
                document.getElementById('MainContent_grdPressureFowrding_txtForwardMeanReading_' + spId).value = parseFloat(meanReading).toFixed(4);
                var error = nominal - meanReading;
                document.getElementById('MainContent_grdPressureFowrding_txtForwardError_' + spId).value = parseFloat(error).toFixed(4);
            });
            $('.reversedPressurecal').blur(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdPressureFowrding
                var tnominal = document.getElementById('MainContent_grdPressureFowrding_txtnominalsizeForward_' + spId).value;
                var t1 = document.getElementById('MainContent_grdPressureFowrding_txtReverseR1_' + spId).value;
                var t2 = document.getElementById('MainContent_grdPressureFowrding_txtReverseR2_' + spId).value;
                var t3 = document.getElementById('MainContent_grdPressureFowrding_txtReverseR3_' + spId).value;
                var nominal = 0;
                var r1 = 0;
                var r2 = 0;
                var r3 = 0;

                if (tnominal != "") {
                    nominal = parseFloat(tnominal);
                }

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                if (t3 != "") {
                    r3 = parseFloat(t3);
                }


                var resultSum = (parseFloat(r1) + parseFloat(r2) + parseFloat(r3));
                var meanReading = resultSum / 3;
                document.getElementById('MainContent_grdPressureFowrding_txtReverseMeanReading_' + spId).value = parseFloat(meanReading).toFixed(4);
                var error = nominal - meanReading;
                document.getElementById('MainContent_grdPressureFowrding_txtReverseError_' + spId).value = parseFloat(error).toFixed(4);
            });
            $('.reversedPressurecal').keyup(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdPressureFowrding
                var tnominal = document.getElementById('MainContent_grdPressureFowrding_txtnominalsizeForward_' + spId).value;
                var t1 = document.getElementById('MainContent_grdPressureFowrding_txtReverseR1_' + spId).value;
                var t2 = document.getElementById('MainContent_grdPressureFowrding_txtReverseR2_' + spId).value;
                var t3 = document.getElementById('MainContent_grdPressureFowrding_txtReverseR3_' + spId).value;
                var nominal = 0;
                var r1 = 0;
                var r2 = 0;
                var r3 = 0;

                if (tnominal != "") {
                    nominal = parseFloat(tnominal);
                }

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                if (t3 != "") {
                    r3 = parseFloat(t3);
                }


                var resultSum = (parseFloat(r1) + parseFloat(r2) + parseFloat(r3));
                var meanReading = resultSum / 3;
                document.getElementById('MainContent_grdPressureFowrding_txtReverseMeanReading_' + spId).value = parseFloat(meanReading).toFixed(4);
                var error = nominal - meanReading;
                document.getElementById('MainContent_grdPressureFowrding_txtReverseError_' + spId).value = parseFloat(error).toFixed(4);
            });
            $('.plunger1').blur(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdPressureFowrding
                var tnominal = document.getElementById('MainContent_grdPlunger1_txtnominalsizePlunger_' + spId).value;
                var t1 = document.getElementById('MainContent_grdPlunger1_txtPlungerR1_' + spId).value;
                var t2 = document.getElementById('MainContent_grdPlunger1_txtPlungerR2_' + spId).value;
                var nominal = 0;
                var r1 = 0;
                var r2 = 0;
                if (tnominal != "") {
                    nominal = parseFloat(tnominal);
                }

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                var resultSum = (parseFloat(r1) + parseFloat(r2));
                var meanReading = resultSum / 2;
                document.getElementById('MainContent_grdPlunger1_txtMeanReading_' + spId).value = parseFloat(meanReading).toFixed(4);
                var error = nominal - meanReading;
                document.getElementById('MainContent_grdPlunger1_txterror_' + spId).value = parseFloat(error).toFixed(4);
            });
            $('.plunger1').keyup(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdPressureFowrding
                var tnominal = document.getElementById('MainContent_grdPlunger1_txtnominalsizePlunger_' + spId).value;
                var t1 = document.getElementById('MainContent_grdPlunger1_txtPlungerR1_' + spId).value;
                var t2 = document.getElementById('MainContent_grdPlunger1_txtPlungerR2_' + spId).value;
                var nominal = 0;
                var r1 = 0;
                var r2 = 0;
                if (tnominal != "") {
                    nominal = parseFloat(tnominal);
                }

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                var resultSum = (parseFloat(r1) + parseFloat(r2));
                var meanReading = resultSum / 2;
                document.getElementById('MainContent_grdPlunger1_txtMeanReading_' + spId).value = parseFloat(meanReading).toFixed(4);
                var error = nominal - meanReading;
                document.getElementById('MainContent_grdPlunger1_txterror_' + spId).value = parseFloat(error).toFixed(4);
            });
            $('.plunger2').blur(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdPressureFowrding
                var tnominal = document.getElementById('MainContent_grdPlunger2_txtnominalsizePlunger_' + spId).value;
                var t1 = document.getElementById('MainContent_grdPlunger2_txtPlungerR1_' + spId).value;
                var t2 = document.getElementById('MainContent_grdPlunger2_txtPlungerR2_' + spId).value;
                var nominal = 0;
                var r1 = 0;
                var r2 = 0;
                if (tnominal != "") {
                    nominal = parseFloat(tnominal);
                }

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                var resultSum = (parseFloat(r1) + parseFloat(r2));
                var meanReading = resultSum / 2;
                document.getElementById('MainContent_grdPlunger2_txtMeanReading_' + spId).value = parseFloat(meanReading).toFixed(4);
                var error = nominal - meanReading;
                document.getElementById('MainContent_grdPlunger2_txterror_' + spId).value = parseFloat(error).toFixed(4);
            });
            $('.plunger2').keyup(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdPressureFowrding
                var tnominal = document.getElementById('MainContent_grdPlunger2_txtnominalsizePlunger_' + spId).value;
                var t1 = document.getElementById('MainContent_grdPlunger2_txtPlungerR1_' + spId).value;
                var t2 = document.getElementById('MainContent_grdPlunger2_txtPlungerR2_' + spId).value;
                var nominal = 0;
                var r1 = 0;
                var r2 = 0;
                if (tnominal != "") {
                    nominal = parseFloat(tnominal);
                }

                if (t1 != "") {
                    r1 = parseFloat(t1);
                }
                if (t2 != "") {
                    r2 = parseFloat(t2);
                }
                var resultSum = (parseFloat(r1) + parseFloat(r2));
                var meanReading = resultSum / 2;
                document.getElementById('MainContent_grdPlunger2_txtMeanReading_' + spId).value = parseFloat(meanReading).toFixed(4);
                var error = nominal - meanReading;
                document.getElementById('MainContent_grdPlunger2_txterror_' + spId).value = parseFloat(error).toFixed(4);
            });
            $('.vernierextcalculation').blur(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdVernier_txtexternalErrorTopR1_0
                var texEtopR1 = document.getElementById('MainContent_grdVernier_txtexternalErrorTopR1_' + spId).value;
                var texEbottomR1 = document.getElementById('MainContent_grdVernier_txtexternalErrorBottomR1_' + spId).value;
                var texEtopR2 = document.getElementById('MainContent_grdVernier_txtexternalErrorTopR2_' + spId).value;
                var texEbottomR2 = document.getElementById('MainContent_grdVernier_txtexternalErrorBottomR2_' + spId).value;
                var texEAvgtop = document.getElementById('MainContent_grdVernier_txtExternalAvgErrorTop_' + spId).value;
                var texEAvgbottom = document.getElementById('MainContent_grdVernier_txtExternalAvgErrorBottom_' + spId).value;
                var texCalErrortop = document.getElementById('MainContent_grdVernier_hfExtError1Top_' + spId).value;
                var texCalErrorbottom = document.getElementById('MainContent_grdVernier_hfExtError1Bottom_' + spId).value;
                var topR1 = 0;
                var bottomR1 = 0;
                var topR2 = 0;
                var bottomR2 = 0;
                var extAvgtop = 0;
                var extAvgbottom = 0;
                var errortopex = 0;
                var errorbottomex = 0;
                if (texEtopR1 != "") {
                    topR1 = parseFloat(texEtopR1);
                }
                if (texEbottomR1 != "") {
                    bottomR1 = parseFloat(texEbottomR1);
                }
                if (texEtopR2 != "") {
                    topR2 = parseFloat(texEtopR2);
                }
                if (texEbottomR2 != "") {
                    bottomR2 = parseFloat(texEbottomR2);
                }
                //Calculation Part
                var topAvgVal = topR1 + topR2;
                topAvgVal = topAvgVal / 2;
                var bottomAvgVal = bottomR1 + bottomR2;
                bottomAvgVal = bottomAvgVal / 2;
                extAvgtop = parseFloat(topAvgVal).toFixed(4);
                extAvgbottom = parseFloat(bottomAvgVal).toFixed(4);
                document.getElementById('MainContent_grdVernier_txtExternalAvgErrorTop_' + spId).value = parseFloat(topAvgVal);
                document.getElementById('MainContent_grdVernier_txtExternalAvgErrorBottom_' + spId).value = parseFloat(bottomAvgVal);

                var nominalSize = document.getElementById('MainContent_grdVernier_txtnominalsizeVernier_' + spId).value;
                errortopex = parseFloat(nominalSize) - parseFloat(extAvgtop);

                errorbottomex = parseFloat(nominalSize) - parseFloat(extAvgbottom);

                document.getElementById('MainContent_grdVernier_hfExtError1Top_' + spId).value = parseFloat(errortopex).toFixed(4);
                document.getElementById('MainContent_grdVernier_hfExtError1Bottom_' + spId).value = parseFloat(errorbottomex).toFixed(4);
            });
            $('.vernierextcalculation').keyup(function () {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //MainContent_grdVernier_txtexternalErrorTopR1_0
                var texEtopR1 = document.getElementById('MainContent_grdVernier_txtexternalErrorTopR1_' + spId).value;
                var texEbottomR1 = document.getElementById('MainContent_grdVernier_txtexternalErrorBottomR1_' + spId).value;
                var texEtopR2 = document.getElementById('MainContent_grdVernier_txtexternalErrorTopR2_' + spId).value;
                var texEbottomR2 = document.getElementById('MainContent_grdVernier_txtexternalErrorBottomR2_' + spId).value;
                var texEAvgtop = document.getElementById('MainContent_grdVernier_txtExternalAvgErrorTop_' + spId).value;
                var texEAvgbottom = document.getElementById('MainContent_grdVernier_txtExternalAvgErrorBottom_' + spId).value;
                var texCalErrortop = document.getElementById('MainContent_grdVernier_hfExtError1Top_' + spId).value;
                var texCalErrorbottom = document.getElementById('MainContent_grdVernier_hfExtError1Bottom_' + spId).value;
                var topR1 = 0;
                var bottomR1 = 0;
                var topR2 = 0;
                var bottomR2 = 0;
                var extAvgtop = 0;
                var extAvgbottom = 0;
                var errortopex = 0;
                var errorbottomex = 0;
                if (texEtopR1 != "") {
                    topR1 = parseFloat(texEtopR1);
                }
                if (texEbottomR1 != "") {
                    bottomR1 = parseFloat(texEbottomR1);
                }
                if (texEtopR2 != "") {
                    topR2 = parseFloat(texEtopR2);
                }
                if (texEbottomR2 != "") {
                    bottomR2 = parseFloat(texEbottomR2);
                }
                //Calculation Part
                var topAvgVal = topR1 + topR2;
                topAvgVal = topAvgVal / 2;
                var bottomAvgVal = bottomR1 + bottomR2;
                bottomAvgVal = bottomAvgVal / 2;
                extAvgtop = parseFloat(topAvgVal).toFixed(4);
                extAvgbottom = parseFloat(bottomAvgVal).toFixed(4);
                document.getElementById('MainContent_grdVernier_txtExternalAvgErrorTop_' + spId).value = parseFloat(topAvgVal);
                document.getElementById('MainContent_grdVernier_txtExternalAvgErrorBottom_' + spId).value = parseFloat(bottomAvgVal);

                var nominalSize = document.getElementById('MainContent_grdVernier_txtnominalsizeVernier_' + spId).value;
                errortopex = parseFloat(nominalSize) - parseFloat(extAvgtop);

                errorbottomex = parseFloat(nominalSize) - parseFloat(extAvgbottom);

                document.getElementById('MainContent_grdVernier_hfExtError1Top_' + spId).value = parseFloat(errortopex).toFixed(4);
                document.getElementById('MainContent_grdVernier_hfExtError1Bottom_' + spId).value = parseFloat(errorbottomex).toFixed(4);
            });

        });
        $(document).ready(function () {

            $('.autoFillNR').keyup(function () {

                var isAutoFill = $("#<%= hfIsAutoFill.ClientID  %>").val();
                //isAutoFill = $("#<%= hfIsAutoFill.ClientID  %>").val(); //document.getElementById("hfIsAutoFill").value

                if (isAutoFill == "yes") {
                    var $eleId = $(this).attr("id");
                    var splitId = $eleId;
                    var sp = splitId.split('_');
                    var spId = sp[3];
                    var gaugeGridName = sp[1];
                    if (gaugeGridName == "grdCalibResult") {

                        var t1 = document.getElementById('MainContent_grdCalibResult_txtR1_' + spId).value;
                        document.getElementById('MainContent_grdCalibResult_txtR2_' + spId).value = t1;
                        document.getElementById('MainContent_grdCalibResult_txtR3_' + spId).value = t1;

                    }
                    else if (gaugeGridName == "grdVernier") {
                        // After discuss will work
                    }
                    else if (gaugeGridName == "grdPressureFowrding") {

                        var t1 = document.getElementById('MainContent_grdPressureFowrding_txtForwardR1_' + spId).value;
                        document.getElementById('MainContent_grdPressureFowrding_txtForwardR2_' + spId).value = t1;
                        document.getElementById('MainContent_grdPressureFowrding_txtForwardR3_' + spId).value = t1;
                        // Need to discussion to any suggestion
                        document.getElementById('MainContent_grdPressureFowrding_txtReverseR1_' + spId).value = t1;
                        document.getElementById('MainContent_grdPressureFowrding_txtReverseR2_' + spId).value = t1;
                        document.getElementById('MainContent_grdPressureFowrding_txtReverseR3_' + spId).value = t1;
                    }
                    else if (gaugeGridName == "grdLeverBore") {
                        var t1 = document.getElementById('MainContent_grdLeverBore_txtUpErrorpR1_' + spId).value;
                        document.getElementById('MainContent_grdLeverBore_txtUpErrorpR2_' + spId).value = t1;
                        document.getElementById('MainContent_grdLeverBore_txtDownErrorR1_' + spId).value = t1;
                        document.getElementById('MainContent_grdLeverBore_txtDownErrorR2_' + spId).value = t1;


                    }
                    else if (gaugeGridName == "grdPlunger1") {
                        var t1 = document.getElementById('MainContent_grdPlunger1_txtPlungerR1_' + spId).value;
                        document.getElementById('MainContent_grdPlunger1_txtPlungerR2_' + spId).value = t1;

                    }
                    else if (gaugeGridName == "grdPlunger2") {
                        var t1 = document.getElementById('MainContent_grdPlunger2_txtPlungerR1_' + spId).value;
                        document.getElementById('MainContent_grdPlunger2_txtPlungerR2_' + spId).value = t1;
                    }

                }
            });
       });


    </script>
    <h3><%: Title %>Certificate</h3>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="col-md-12">
                <div class="form-horizontal">
                    <div class="form-group">

                        <div class="col-md-10" style="text-align: right" runat="server" visible="false">
                            <b>
                                <asp:Label ID="lbl1" runat="server" Text="No. of Count :">
                                </asp:Label>
                                <asp:Label ID="lblcnt" runat="server">
                                </asp:Label></b>


                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:GridView ID="grdCertificate" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                                    PageSize="50" OnPageIndexChanging="grdCertificate_PageIndexChanging" HeaderStyle-Wrap="false">
                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:BoundField DataField="gauge_name" HeaderText="Gauge" />
                                        <asp:BoundField DataField="certificate_no" HeaderText="Certificate No" />
                                        <asp:BoundField DataField="type_of_gauge" HeaderText="Gauge Type" />
                                        <asp:BoundField DataField="size_range" HeaderText="Size Range" />
                                        <asp:BoundField DataField="satifactory" HeaderText="Condition Of Receipt" />
                                        <asp:BoundField DataField="identification_mark_by" HeaderText="Identification Mark By" />
                                        <asp:BoundField DataField="CertificationDone" HeaderText="Certification Done" />
                                        <asp:BoundField DataField="customer_name" HeaderText="Customer" />
                                        <asp:TemplateField HeaderText="Create Certificate" HeaderStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="updpaEdit" runat="server">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnCerateCertificate" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnCerateCertificate" runat="server" CommandArgument='<%# Eval("id") + ","+Eval("certificate_id") %>' OnClick="btnCerateCertificate_Click" Text="Create Certificate"
                                                            Enabled="false" CssClass="btn btn-primary" title="Create Certificate"></asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:HiddenField ID="hfIsAutoFill" runat="server" Value="<%$appSettings:IsAutoFill %>"></asp:HiddenField>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Auto">

                                <asp:GridView ID="grdCalibResult" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                                    PageSize="100" HeaderStyle-Wrap="false">
                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Nominal Size in mm" ItemStyle-Font-Bold="true" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Larger">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtnominalsize" Text='<%# Eval("nominal_size")%>' onkeypress="return isNumberKey(this, event);" ReadOnly="true" MaxLength="15" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="R1" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtR1" Text='<%# Eval("r1")%>' onkeypress="return isNumberKey(this, event);" class="calculation autoFillNR" MaxLength="15" Width="120" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtR1"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="R1 is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="R2" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtR2" Text='<%# Eval("r2")%>' onkeypress="return isNumberKey(this, event);" class="calculation" onchange="Call()" MaxLength="15" Width="120" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtR2"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="R2 is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="R3" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtR3" Text='<%# Eval("r3")%>' onkeypress="return isNumberKey(this, event);" class="calculation" MaxLength="15" Width="120" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtr3"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Field is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Observed in mm" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtObserved" Text='<%# Eval("obsereved")%>' onkeypress="return isNumberKey(this, event);" class="calculation" MaxLength="15" Width="120" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtObserved"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Field is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Error in mm" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtError" Text='<%# Eval("error")%>' onkeypress="return isNumberKey(this, event);" class="calculation" MaxLength="15" Width="120" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtError"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Field is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                    </Columns>
                                    <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                </asp:GridView>

                                <asp:GridView ID="grdVernier" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                                    PageSize="100" HeaderStyle-Wrap="false" OnRowCreated="grdVernier_RowCreated">
                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" HeaderText="Nominal Size" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Larger">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtnominalsizeVernier" Text='<%# Eval("nominal_size")%>' onkeypress="return isNumberKey(this, event);" Width="80" ReadOnly="true" MaxLength="15" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Top" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtexternalErrorTopR1" Text='<%# Eval("ExternalErrorTopR1")%>' onkeypress="return isNumberKey(this, event);" class="vernierextcalculation" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtexternalErrorTopR1"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Top is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bottom" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtexternalErrorBottomR1" Text='<%# Eval("ExternalErrorBottomR1")%>' onkeypress="return isNumberKey(this, event);" class="vernierextcalculation" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtexternalErrorBottomR1"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Bottom is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Top" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtexternalErrorTopR2" Text='<%# Eval("ExternalErrorTopR2")%>' onkeypress="return isNumberKey(this, event);" class="vernierextcalculation" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtexternalErrorTopR2"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Top is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bottom" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtexternalErrorBottomR2" Text='<%# Eval("ExternalErrorBottomR2")%>' onkeypress="return isNumberKey(this, event);" class="vernierextcalculation" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtexternalErrorBottomR2"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Bottom is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Top" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtExternalAvgErrorTop" Text='<%# Eval("ExternalAvgErrorTop")%>' onkeypress="return isNumberKey(this, event);" class="vernierextcalculation" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtExternalAvgErrorTop"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Top is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Bottom" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtExternalAvgErrorBottom" Text='<%# Eval("ExternalAvgErrorBottom")%>' onkeypress="return isNumberKey(this, event);" class="vernierextcalculation" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtExternalAvgErrorTop"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Top is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--Internal Data below--%>
                                        <asp:TemplateField HeaderText="Top" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInternalErrorTopR1" Text='<%# Eval("InternalErrorTopR1")%>' onkeypress="return isNumberKey(this, event);" class="vernierintcalculation" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtexternalErrorTopR1"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Top is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bottom" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInternalErrorBottomR1" Text='<%# Eval("InternalErrorBottomR1")%>' onkeypress="return isNumberKey(this, event);" class="vernierintcalculation" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtexternalErrorBottomR1"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Bottom is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Top" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInternalErrorTopR2" Text='<%# Eval("InternalErrorTopR2")%>' onkeypress="return isNumberKey(this, event);" class="vernierintcalculation" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtexternalErrorTopR2"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Top is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bottom" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInternalErrorBottomR2" Text='<%# Eval("InternalErrorBottomR2")%>' onkeypress="return isNumberKey(this, event);" class="vernierintcalculation" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtInternalErrorBottomR2"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Bottom is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Top" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInternalAvgErrorTop" Text='<%# Eval("InternalAvgErrorTop")%>' onkeypress="return isNumberKey(this, event);" class="vernierintcalculation" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtExternalAvgErrorTop"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Top is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Bottom" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtInternalAvgErrorBottom" Text='<%# Eval("InternalAvgErrorBottom")%>' onkeypress="return isNumberKey(this, event);" class="vernierintcalculation" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtExternalAvgErrorTop"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Top is required." />

                                                <asp:HiddenField ID="hfExtError1Top" runat="server" Value='<%# Eval("calculated_ex_error_top")%>'></asp:HiddenField>
                                                <asp:HiddenField ID="hfExtError1Bottom" runat="server" Value='<%# Eval("calculated_ex_error_bottom")%>'></asp:HiddenField>
                                                <asp:HiddenField ID="hfIntError1Top" runat="server" Value='<%# Eval("calculated_in_error_top")%>'></asp:HiddenField>
                                                <asp:HiddenField ID="hfIntError1Bottom" runat="server" Value='<%# Eval("calculated_in_error_bottom")%>'></asp:HiddenField>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                </asp:GridView>

                                <asp:GridView ID="grdLeverBore" runat="server" Width="80%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                                    PageSize="100" HeaderStyle-Wrap="false" OnRowCreated="grdLeverBore_RowCreated">
                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" ItemStyle-Width="30" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Larger">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtnominalsizeLeverBore" Text='<%# Eval("nominal_size")%>' onkeypress="return isNumberKey(this, event);" Width="150" ReadOnly="true" MaxLength="15" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="R1" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUpErrorpR1" Text='<%# Eval("UpErrorpR1")%>' onkeypress="return isNumberKey(this, event);" class="leverborecalculation autoFillNR" MaxLength="15" Width="70" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtUpErrorpR1"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="R2" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUpErrorpR2" Text='<%# Eval("UpErrorpR2")%>' onkeypress="return isNumberKey(this, event);" class="leverborecalculation" MaxLength="15" Width="70" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtUpErrorpR2"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="R1" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDownErrorR1" Text='<%# Eval("DownErrorR1")%>' onkeypress="return isNumberKey(this, event);" class="leverborecalculation" MaxLength="15" Width="70" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtDownErrorR1"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="R2" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDownErrorR2" Text='<%# Eval("DownErrorR2")%>' onkeypress="return isNumberKey(this, event);" class="leverborecalculation" MaxLength="15" Width="70" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtDownErrorR1"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Upward" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUpward" Text='<%# Eval("Upward")%>' onkeypress="return isNumberKey(this, event);" class="leverborecalculation" MaxLength="15" Width="70" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtUpward"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Downward" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDownward" Text='<%# Eval("Downward")%>' onkeypress="return isNumberKey(this, event);" class="leverborecalculation" MaxLength="15" Width="70" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtDownward"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                                <asp:HiddenField ID="hfUpError" runat="server" Value='<%# Eval("error_up")%>'></asp:HiddenField>
                                                <asp:HiddenField ID="hfDownError" runat="server" Value='<%# Eval("error_down")%>'></asp:HiddenField>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                </asp:GridView>

                                <asp:GridView ID="grdFeeler" runat="server" Width="80%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                                    PageSize="100" HeaderStyle-Wrap="false">
                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Nominal Thikness" ItemStyle-Font-Bold="true" ItemStyle-Width="30" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Larger">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtnominalsizeFeeler" Text='<%# Eval("nominal_size")%>' onkeypress="return isNumberKey(this, event);" Width="150" ReadOnly="true" MaxLength="15" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Observed Thikness" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtObserved" Text='<%# Eval("observed")%>' onkeypress="return isNumberKey(this, event);" class="feelercalculation" MaxLength="15" Width="70" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtObserved"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Variation in Thikness" HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtvariation" Text='<%# Eval("variation")%>' onkeypress="return isNumberKey(this, event);" class="feelercalculation" MaxLength="15" Width="70" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtvariation"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                </asp:GridView>

                            </asp:Panel>
                        </div>
                        <div class="col-md-6">
                            <asp:GridView ID="grdPlunger1" runat="server" Width="50%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                                PageSize="100" HeaderStyle-Wrap="false">
                                <HeaderStyle CssClass="header" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" HeaderText="Nominal Size" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Larger">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtnominalsizePlunger" Text='<%# Eval("nominal_size")%>' onkeypress="return isNumberKey(this, event);" Width="80" ReadOnly="true" MaxLength="15" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="R1" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPlungerR1" Text='<%# Eval("PlungerR1")%>' onkeypress="return isNumberKey(this, event);" class="plunger1 autoFillNR" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtPlungerR1"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="R2" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPlungerR2" Text='<%# Eval("PlungerR2")%>' onkeypress="return isNumberKey(this, event);" class="plunger1" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtPlungerR2"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Mean Reading" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMeanReading" Text='<%# Eval("PlungerMeanReding")%>' onkeypress="return isNumberKey(this, event);" class="plunger1" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtMeanReading"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Top is required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Error">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txterror" Text='<%# Eval("PlungerError")%>' onkeypress="return isNumberKey(this, event);" class="plunger1" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txterror"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Top is required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                        <div class="col-md-6" style="margin-left: 0px">
                            <asp:GridView ID="grdPlunger2" runat="server" Width="50%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                                PageSize="100" HeaderStyle-Wrap="false">
                                <HeaderStyle CssClass="header" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" HeaderText="Nominal Size" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Larger">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtnominalsizePlunger" Text='<%# Eval("nominal_size")%>' onkeypress="return isNumberKey(this, event);" Width="80" ReadOnly="true" MaxLength="15" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="R1" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPlungerR1" Text='<%# Eval("PlungerR1")%>' onkeypress="return isNumberKey(this, event);" class="plunger2 autoFillNR" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtPlungerR1"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="R2" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPlungerR2" Text='<%# Eval("PlungerR2")%>' onkeypress="return isNumberKey(this, event);" class="plunger2" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtPlungerR2"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Mean Reading" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMeanReading" Text='<%# Eval("PlungerMeanReding")%>' onkeypress="return isNumberKey(this, event);" class="plunger2" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtMeanReading"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Error">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txterror" Text='<%# Eval("PlungerError")%>' onkeypress="return isNumberKey(this, event);" class="plunger2" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txterror"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                            </asp:GridView>
                        </div>


                        <div class="col-md-12">
                            <asp:GridView ID="grdPressureFowrding" runat="server" Width="50%" AutoGenerateColumns="false" CssClass="table table-bordered table-responsive"
                                HeaderStyle-Wrap="false">
                                <HeaderStyle CssClass="header" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" HeaderText="Forwarding Reading" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Larger">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtnominalsizeForward" Text='<%# Eval("nominal_size")%>' onkeypress="return isNumberKey(this, event);" Width="80" ReadOnly="true" MaxLength="15" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="R1" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtForwardR1" Text='<%# Eval("ForwardR1")%>' onkeypress="return isNumberKey(this, event);" class="autoFillNR forwardPressurecal reversedPressurecal" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtForwardR1"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="R2" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtForwardR2" Text='<%# Eval("ForwardR2")%>' onkeypress="return isNumberKey(this, event);" class="forwardPressurecal" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtForwardR2"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="R3" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtForwardR3" Text='<%# Eval("ForwardR3")%>' onkeypress="return isNumberKey(this, event);" class="forwardPressurecal" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtForwardR3"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Mean Reading" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtForwardMeanReading" Text='<%# Eval("ForwardMeanReding")%>' onkeypress="return isNumberKey(this, event);" class="forwardPressurecal" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtForwardMeanReading"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Error">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtForwardError" Text='<%# Eval("ForwardError")%>' onkeypress="return isNumberKey(this, event);" class="forwardPressurecal" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtForwardError"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="R1" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReverseR1" Text='<%# Eval("ReverseR1")%>' onkeypress="return isNumberKey(this, event);" class="reversedPressurecal" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtReverseR1"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="R2" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReverseR2" Text='<%# Eval("ReverseR2")%>' onkeypress="return isNumberKey(this, event);" class="reversedPressurecal" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtReverseR2"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="R3" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReverseR3" Text='<%# Eval("ReverseR3")%>' onkeypress="return isNumberKey(this, event);" class="reversedPressurecal" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtReverseR3"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mean Reading" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReverseMeanReading" Text='<%# Eval("ReverseMeanReding")%>' onkeypress="return isNumberKey(this, event);" class="reversedPressurecal" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtReverseMeanReading"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Error">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReverseError" Text='<%# Eval("ReverseError")%>' onkeypress="return isNumberKey(this, event);" class="reversedPressurecal" MaxLength="15" Width="65" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtReverseError"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Required." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                            </asp:GridView>

                            <table class="table table-bordered table-responsive" runat="server" id="tblforAttribute">
                                <tr class="header">
                                    <td>
                                        <asp:Label runat="server" Text="No./Make"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label runat="server" Text="Nominal Size In"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label runat="server" Text="Specified Size In"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label runat="server" Text="Observed Size In"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="header">
                                    <td></td>
                                    <td>
                                        <asp:Label runat="server" Text="GO"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="NO GO"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="GO"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="NO GO"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="GO"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="NO GO"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Label runat="server" ID="lblGOLowerSize" Text="GOLowerSize"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblNOGOHigherSize" Text="NOGOHigherSize"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblGOMinus" Text="GOMinus"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblNOGOMinus" Text="NOGOMinus"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblmakeforAttribute"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:Label runat="server" ID="lblGoPlus" Text="GOPlus"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblNOGOPlus" Text="NOGOPlus"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtGoObservedSize" Width="90px" Text="0" class="attributeCal"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtNoGoObservedSize" Width="90px" Text="0" class="attributeCal"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:Label runat="server" ID="lblWereLimit" Text="WereLimit"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>

                            <table class="table table-bordered table-responsive" runat="server" id="tblThredPlugGauge">
                                <tr class="header">
                                    <td>
                                        <asp:Label runat="server" Text="No./Make"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label runat="server" Text="Nominal Size In"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label runat="server" Text="Specified Size In"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label runat="server" Text="Observed Size In"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="header">
                                    <td></td>
                                    <td>
                                        <asp:Label runat="server" Text="GO"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="NO GO"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="GO"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="NO GO"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="GO"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="NO GO"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Label runat="server" ID="lblGOLowerSizeTPG" Text="GOLowerSize"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblNOGOHigherSizeTPG" Text="NOGOHigherSize"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblGOMinusTPG" Text="GOMinus"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblNOGOMinusTPG" Text="NOGOMinus"></asp:Label>
                                    </td>
                                    <td>
                                     <asp:Label runat="server" ID="Label1" Text="Maj. Dia"></asp:Label>
                                     <asp:TextBox runat="server" ID="txtGOMajDiaTPG" Width="90px" Text="0" class="attributeCal"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="Label2" Text="Maj. Dia"></asp:Label>
                                     <asp:TextBox runat="server" ID="txtNOGOMajDiaTPG" Width="90px" Text="0" class="attributeCal"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblmakeforTPG"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:Label runat="server" ID="lblGoPlusTPG" Text="GOPlus"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblNOGOPlusTPG" Text="NOGOPlus"></asp:Label>
                                    </td>
                                    <td>
                                      <asp:Label runat="server" ID="Label3" Text="Eff. Dia"></asp:Label>&nbsp;
                                      <asp:TextBox runat="server" ID="txtGoObservedSizeTPG" Width="90px" Text="0" class="attributeCal"></asp:TextBox>
                                    </td>
                                    <td>
                                       <asp:Label runat="server" ID="Label4" Text="Eff. Dia"></asp:Label>&nbsp;
                                       <asp:TextBox runat="server" ID="txtNoGoObservedSizeTPG" Width="90px" Text="0" class="attributeCal"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:Label runat="server" ID="lblWereLimitTPG" Text="WereLimit"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>


                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <asp:PlaceHolder runat="server" ID="PlaceHolder2" Visible="false">
                                <p class="text-danger">
                                    <asp:Literal runat="server" ID="Literal2" />
                                </p>
                            </asp:PlaceHolder>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Total Error<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txttotalerror" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ValidationGroup="a" SetFocusOnError="true"
                                                ControlToValidate="txttotalerror" Display="Dynamic" CssClass="text-danger" ErrorMessage="Total Error field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Face // sm<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtfacesm" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ValidationGroup="a" SetFocusOnError="true"
                                                ControlToValidate="txtfacesm" Display="Dynamic" CssClass="text-danger" ErrorMessage="Face // sm field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Visual<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtvisual" placeholder="OK/Not OK" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ValidationGroup="a" SetFocusOnError="true"
                                                ControlToValidate="txtvisual" Display="Dynamic" CssClass="text-danger" ErrorMessage="Visual field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Gauge<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtGauge" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:Label ID="lblCertificateId" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblcertificateIdForEdit" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="lblPermissableErrorFor" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Type Of Gauge</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtTypeOfgauge" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Identification</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtIdentification" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Mfg.No</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtMfgNo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Make</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtMake" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Size</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtSize" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Condition Of Gauge at Receipt</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtConditionOfReceipt" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Test Purpose</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtTestPurpose" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">IS Ref.(Guiedline)</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtIsrefGuideLine" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Calibration Method Number</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtCalibrationMethodNumber" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Calibration Carried Out at<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtCalibCarriedOutat" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Uncertinity Of Measurment</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtuncertintiy" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <asp:PlaceHolder runat="server" ID="PlaceHolder3" Visible="false">
                                <p class="text-danger">
                                    <asp:Literal runat="server" ID="Literal3" />
                                </p>
                            </asp:PlaceHolder>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>

                                    <div class="form-group" runat="server" id="divstanderd">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Standerd<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtstanderd" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ValidationGroup="a" SetFocusOnError="true"
                                                ControlToValidate="txtstanderd" Display="Dynamic" CssClass="text-danger" ErrorMessage="Standerd field is required." />

                                        </div>
                                    </div>

                                    <div runat="server" id="divVernier">
                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-md-3 control-label">Ext.<b style="color: Red"> *</b></asp:Label>
                                            <div class="col-md-9">
                                                <asp:TextBox ID="txtExt" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ValidationGroup="a" SetFocusOnError="true"
                                                    ControlToValidate="txtExt" Display="Dynamic" CssClass="text-danger" ErrorMessage="Ext. field is required." />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-md-3 control-label">Int.<b style="color: Red"> *</b></asp:Label>
                                            <div class="col-md-9">
                                                <asp:TextBox ID="txtInt" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ValidationGroup="a" SetFocusOnError="true"
                                                    ControlToValidate="txtInt" Display="Dynamic" CssClass="text-danger" ErrorMessage="Int. field is required." />

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-md-3 control-label">Depth<b style="color: Red"> *</b></asp:Label>
                                            <div class="col-md-9">
                                                <asp:TextBox ID="txtDepth" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ValidationGroup="a" SetFocusOnError="true"
                                                    ControlToValidate="txtDepth" Display="Dynamic" CssClass="text-danger" ErrorMessage="Depth field is required." />

                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Description</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtdesc" runat="server" CssClass="form-control" TextMode="MultiLine" Height="80"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator runat="server" ValidationGroup="a" SetFocusOnError="true"
                                                ControlToValidate="txtdesc" Display="Dynamic" CssClass="text-danger" ErrorMessage="Description field is required." />--%>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Refrence/Dc.No</asp:Label>
                                        <div class="col-md-9">

                                            <asp:TextBox ID="txtRefrenceDcno" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Date Of Refrence<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtRefrenceDate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Date Of Receipt<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtDateOfReceipt" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <div class="form-group">
                                                <asp:Label runat="server" CssClass="col-md-3 control-label">Frequency Type<b style="color: Red"> *</b></asp:Label>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="txtFrequencyType" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" CssClass="col-md-3 control-label">Calibration Frequency<b style="color: Red"> *</b></asp:Label>
                                                <div class="col-md-9">
                                                    <asp:TextBox runat="server" ReadOnly="true" TabIndex="6" MaxLength="2" ID="txtCalibrationFrequency" CssClass="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" CssClass="col-md-3 control-label">Date Of Calibration<b style="color: Red"> *</b></asp:Label>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="txtDateOfCalib" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" CssClass="col-md-3 control-label">Next Calibration Date<b style="color: Red"> *</b></asp:Label>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="txtNextCalibDate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Identification Marked By RML<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtdentificationMarkedByRML" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Master Equipment Used<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtMasterEquipmentUsed" runat="server" Height="120px" TextMode="MultiLine" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSaveCertificate" />
                        <asp:PostBackTrigger ControlID="btnClose" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="col-md-offset-5 col-md-6" style="margin-bottom: 50px">
                            <div class="col-md-2">
                                <asp:Button runat="server" ID="btnSaveCertificate" ValidationGroup="a" OnClick="btnSaveCertificate_Click" Text="Save" CssClass="btn btn-primary" TabIndex="22" />
                            </div>
                            <div class="col-md-3">
                                <asp:Button runat="server" ID="btnClose" Text="Close" OnClick="btnClose_Click" CssClass="btn btn-primary" TabIndex="23" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </asp:View>
    </asp:MultiView>
</asp:Content>

