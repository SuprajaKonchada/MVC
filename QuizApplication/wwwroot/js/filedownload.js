document.addEventListener("DOMContentLoaded", function () {
    const table = document.querySelector(".table");
    const downloadButton = document.getElementById("download-csv");

    downloadButton.addEventListener("click", function () {
        // Function to convert an HTML table to CSV
        function convertToCSV(table) {
            const lines = [];
            const rows = table.querySelectorAll("tr");

            for (const row of rows) {
                const cols = row.querySelectorAll("td, th");
                const line = [];

                for (const col of cols) {
                    line.push(`"${col.innerText.replace(/"/g, '""')}"`);
                }

                lines.push(line.join(","));
            }

            return lines.join("\n");
        }

        function downloadCSV(csv, filename) {
            const blob = new Blob([csv], { type: "text/csv;charset=utf-8;" });
            const url = URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = filename + ".csv";
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }

        const csvContent = convertToCSV(table);
        const filename = "report";
        downloadCSV(csvContent, filename);
    });
});