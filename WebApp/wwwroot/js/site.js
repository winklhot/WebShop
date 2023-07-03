// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function insertSampleLogin() {
    if (document.getElementById("sampleDataCheck").checked) {
        document.getElementById("loginName").value = "tina@test.de";
        document.getElementById("loginPassword").value = "test";

        hashInputValueLogin();
    }
    else {
        document.getElementById("loginName").value = null;
        document.getElementById("loginPassword").value = null;
    }
}

function insertSampleRegister() {
    if (document.getElementById("sampleDataRegisterCheckbox").checked) {
        document.getElementById("registerFirstname").value = "Henning";
        document.getElementById("registerName").value = "Hedgefonds";
        document.getElementById("registerRadio3").checked = true;
        radioCheck(); // Set the binded Message to result of Rb
        document.getElementById("registerEMail").value = "henning" + Math.round(Math.random() * 10000) + ".hedgefonds@test.de";
        document.getElementById("registerStreet").value = "Taunausanlage";
        document.getElementById("registerHouseNumber").value = "12";
        document.getElementById("registerPostalCode").value = "60325";
        document.getElementById("registerCity").value = "Frankfurt";
        document.getElementById("registerCountry").value = "Germany";
        document.getElementById("registerPasswordClear").value = "test";
        document.getElementById("registerPasswordClear2").value = "test";
        document.getElementById("registerAGBs").checked = true;
        hashInputValueRegister();
    }
    else {
        document.getElementById("registerFirstname").value = "";
        document.getElementById("registerName").value = "";
        document.getElementById("registerRadio1").checked = true;
        document.getElementById("registerEMail").value = "";
        document.getElementById("registerStreet").value = "";
        document.getElementById("registerHouseNumber").value = "";
        document.getElementById("registerPostalCode").value = "";
        document.getElementById("registerCity").value = "";
        document.getElementById("registerCountry").value = "";
        document.getElementById("registerPasswordClear").value = "";
        document.getElementById("registerPasswordClear2").value = "";
        document.getElementById("registerAGBs").checked = false;
    }
}

function insertSampleNonCustomer() {
    if (document.getElementById("sampleNonCustomerCheckbox").checked) {
        document.getElementById("nonCustomerFirstname").value = "Hans-Peter";
        document.getElementById("nonCustomerName").value = "Baxxter";
        document.getElementById("nonCustomerEMail").value = "H.P@test.de";
        document.getElementById("nonCustomerStreet").value = "Fischmarkt";
        document.getElementById("nonCustomerHouseNumber").value = "503";
        document.getElementById("nonCustomerPostalCode").value = "26789";
        document.getElementById("nonCustomerCity").value = "Leer";
        document.getElementById("nonCustomerCountry").value = "Germany";
        document.getElementById("nonCustomerAGBs").checked = true;
    }
    else {
        document.getElementById("nonCustomerFirstname").value = null;
        document.getElementById("nonCustomerName").value = null;
        document.getElementById("nonCustomerEMail").value = null;
        document.getElementById("nonCustomerStreet").value = null;
        document.getElementById("nonCustomerHouseNumber").value = null;
        document.getElementById("nonCustomerPostalCode").value = null;
        document.getElementById("nonCustomerCity").value = null;
        document.getElementById("nonCustomerCountry").value = null;
        document.getElementById("nonCustomerAGBs").checked = false;
    }
}

function hashInputValueLogin() {
    //     Get the input element
    var passwordElement = document.getElementById("loginPassword");
    var saltElement = document.getElementById("loginName");

    //         Get the current value of the input element
    var inputValue = passwordElement.value + saltElement.value;

    // Calculate         the SHA-        256 hash of the in        put value
    var hash = CryptoJS.SHA256(inputValue);

    // Update the value of the input element with the has        h
    document.getElementById("hPwd").value = hash;
}

function hashInputValueRegister() {
    //     Get the input element
    var passwordElement = document.getElementById("registerPasswordClear");
    var saltElement = document.getElementById("registerEMail");

    //                 Get the current value of the inpu        t element
    var inputValue = passwordElement.value + saltElement.value;

    // Calculat        e         the SHA-                        256 hash of the in        put value
    var hash = CryptoJS.SHA256(inputValue);

    // Update the value of the input element with the         has        h
    document.getElementById("registerPassword").value = hash;

    //Second Field

    var passwordElement = document.getElementById("registerPasswordClear2");
    var saltElement = document.getElementById("registerEMail");

    var inputValue = passwordElement.value + saltElement.value;

    var hash = CryptoJS.SHA256(inputValue);

    document.getElementById("registerPassword2").value = hash;
}

function radioCheck() {
    if (document.getElementById("registerRadio1").checked) {
        document.getElementById("GenderInput").value = "male";
    }
    else if (document.getElementById("registerRadio2").checked) {
        document.getElementById("GenderInput").value = "female";
    }
    else if (document.getElementById("registerRadio3").checked) {
        document.getElementById("GenderInput").value = "divers";
    }
}

function radioCheckNonCustomer() {
    if (document.getElementById("nonCustomerRadio1").checked) {
        document.getElementById("nonCustomerGenderInput").value = "male";
    }
    else if (document.getElementById("nonCustomerRadio2").checked) {
        document.getElementById("nonCustomerGenderInput").value = "female";
    }
    else if (document.getElementById("nonCustomerregisterRadio3").checked) {
        document.getElementById("nonCustomerGenderInput").value = "divers";
    }
}

function getPDF(number, cfullname, cstreet, chouseNumber, cpostalCode, ccity, ccountry, pdataString) {
    var doc = new jsPDF();

    // Add invoice header information
    doc.setFontSize("Consolas");
    doc.setFontSize(22);
    doc.text("Rechnung", 14, 22);
    doc.setFontSize(12);

    // Add invoice details
    doc.setFontSize(12);
    doc.text(cfullname, 14, 45);
    doc.text(cstreet + " " + chouseNumber, 14, 50);
    doc.text(cpostalCode + " " + ccity, 14, 55);
    doc.text(ccountry, 14, 60);

    doc.text("Re: " + number, 14, 75);
    doc.text("Datum: " + new Date().toLocaleDateString(), 14, 80);
    // Add table for the order positions
    var x = 14;
    var y = 100;

    // Draw the table head
    doc.text("Name", x, y);
    doc.text("Menge", x + 70, y);
    doc.text("Einzelpreis", x + 110, y, 'right');
    doc.text("Gesamtpreis", x + 160, y, 'right');
    y += 2;

    // Draw horizontal lines for the head and each row
    doc.line(x, y, x + 160, y);
    y += 5;

    // Draw the table rows
    var subtotal = 0.00;

    const str = pdataString.split('$');


    for (var i = 0; i < str.length; i++) {
        const word = str[i].split(';');

        doc.text(word[0], x, y);
        doc.text(word[1], x + 70, y);
        doc.text(word[2] + "EUR", x + 110, y, 'right');
        doc.text(word[3] + "EUR", x + 160, y, 'right');
        y += 2;
        doc.line(x, y, x + 160, y);
        y += 5;
        subtotal += parseFloat(word[3].replace(',', '.'));
    }

    y += 10;




    // Draw the subtotal, tax, and total rows
    doc.text("Nettobetrag:", x, y);
    doc.text((subtotal / 1.19).toFixed(2).replace('.', ',') + " EUR", x + 60, y, 'right');

    doc.text("Umsatzsteuer:", x, y + 5,);
    doc.text((subtotal / 1.19 * 0.19).toFixed(2).replace('.', ',') + " EUR", x + 60, y + 5, 'right');

    doc.text("Total:", x, y + 10);
    doc.text((subtotal).toFixed(2).replace('.', ',') + " EUR", x + 60, y + 10, 'right');


    var pdfDataUri = doc.output('dataurlnewwindow');
}

var app = new Vue(
    {
        el: '#app',
        data: {
            message: "Hallo Vue.js",
            port: 5000,
            artikel: [],
            suchbegriff: [],
        },
        methods:
        {
            GetArtikel() {
                if (this.suchbegriff.length < 3) {
                    this.artikel = [];
                    return;
                }

                axios.get("http://localhost:" + this.port + "/api/article/search/" + this.suchbegriff)
                    .then(response => this.artikel = response.data);
            },
            GetAlleArtikel() {
                axios.get("http://localhost:" + this.port + "/api/article")
                    .then(response => this.artikel = response.data);
            },
        },
        computed:
        {
            isDisabled() {
                return this.suchbegriff.length < 4;
            }
        }
    })
