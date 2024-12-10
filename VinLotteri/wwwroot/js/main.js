async function buyTicket(ticketNumber, buyerName) {
    const apiUrl = "/api/tickets/buy"; // Lokal URL
    const ticketData = {
        number: ticketNumber,
        buyerName: buyerName
    };

    try {
        const response = await fetch(apiUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(ticketData)
        });

        if (response.ok) {
            const ticket = await response.json();
            alert(`Lodd kjøpt! Loddnummer: ${ticket.number}, Kjøper: ${ticket.buyerName}`);
        } else {
            const error = await response.text();
            alert(`Feil: ${error}`);
        }
    } catch (err) {
        alert("En feil har oppstått! Vennligst prøv igjen senere.");
    }
}

document.getElementById("buy-ticket-button").addEventListener("click", () => {
    const ticketNumber = document.getElementById("ticket-number").value;
    const buyerName = document.getElementById("buyer-name").value;

    if (ticketNumber && buyerName) {
        buyTicket(parseInt(ticketNumber), buyerName);
    } else {
        alert("Fyll inn både loddnummer og kjøpernavn.");
    }
});

// Funksjon for å hente alle lodd
async function fetchTickets() {
    const apiUrl = "/api/tickets";

    try {
        const response = await fetch(apiUrl);

        if (response.ok) {
            const tickets = await response.json();
            displayTickets(tickets);
        } else {
            alert("Kunne ikke hente lodd. Vennligst prøv igjen senere.");
        }
    } catch (err) {
        alert("Feil! Vennligst prøv igjen senere.");
    }
}

// Funksjon for å vise lodd i en liste
function displayTickets(tickets) {
    const ticketList = document.getElementById("ticket-list");
    ticketList.innerHTML = ""; // Fjern tidligere innhold

    tickets.forEach(ticket => {
        const ticketItem = document.createElement("li");
        ticketItem.textContent = `Loddnummer: ${ticket.number}, Status: ${ticket.status}, Kjøper: ${ticket.buyerName || ""}`;
        ticketList.appendChild(ticketItem);
    });
}

// Funksjon for å trekke en vinner
async function drawWinner() {
    const apiUrl = "/api/tickets/draw";

    try {
        const response = await fetch(apiUrl, { method: "POST" });

        if (response.ok) {
            const result = await response.json();
            alert(`Vinneren er: Loddnummer ${result.winningTicketNumber}, Kjøper: ${result.buyerName}`);
        } else {
            const error = await response.text();
            alert(`Feil: ${error}`);
        }
    } catch (err) {
        alert("Feil! Vennligst prøv igjen senere.");
    }
}

// Legg til event listeners
document.getElementById("draw-winner-button").addEventListener("click", drawWinner);
document.getElementById("fetch-tickets-button").addEventListener("click", fetchTickets);

