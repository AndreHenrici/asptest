namespace AspTest;

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class BoxHub : Hub
{
    // Aktuelle Position als Server-State (vereinfachtes Beispiel)
    private static int TopPos = 100;
    private static int LeftPos = 100;

    public async Task MoveBox(string direction)
    {
        // Einfacher Schrittwert
        int step = 10;

        switch (direction.ToLower())
        {
            case "up":
                TopPos = TopPos - step < 0 ? 0 : TopPos - step;
                break;
            case "down":
                TopPos = TopPos + step > 500 ? 500 : TopPos + step; // Begrenzung anpassen
                break;
            case "left":
                LeftPos = LeftPos - step < 0 ? 0 : LeftPos - step;
                break;
            case "right":
                LeftPos = LeftPos + step > 800 ? 800 : LeftPos + step; // Begrenzung anpassen
                break;
        }

        // Sende neue Position an alle Clients
        await Clients.All.SendAsync("UpdatePosition", TopPos, LeftPos);
    }

    public override async Task OnConnectedAsync()
    {
        // Wenn ein neuer Client verbindet, sende aktuelle Position
        await Clients.Caller.SendAsync("UpdatePosition", TopPos, LeftPos);
        await base.OnConnectedAsync();
    }
}