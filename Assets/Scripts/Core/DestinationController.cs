#region Using Statements
using FTS.Grid;
using System.Collections.Generic;
using UnityEngine;
using FTS.Cards;
using System.Linq;
using UnityEngine.SceneManagement;
#endregion


namespace FTS.Core
{
    enum DestinationTypes
    {
        Shop,
        Event,
        Draft,
        Treasure
    }
    public class DestinationController : MonoBehaviour
    {
        [SerializeField] GameObject destinationPrefab;
        [SerializeField] List<Destination> destinationSO;
        List<Destination> destinations = new List<Destination>();
        //Destination[] destinations;


        private void Start()
        {
            Shuffle();
        }

        internal void SetDestination(List<HexCell> vehicleDestinations)
        {
            foreach (var cell in vehicleDestinations)
            {
                GameObject destinationUI = Instantiate(destinationPrefab);
                Destination destination = GetUnselectedDest();

                cell.IsDestination = true;
                destination.Location = cell.Location;
                destinationUI.transform.SetParent(cell.transform, false);
                destinationUI.GetComponent<DestinationUI>().SetType(destination);
                destinations.Add(destination);
            }
        }

        internal void SetDestination(HexCell vehicleDestination)
        {
            GameObject destinationUI = Instantiate(destinationPrefab);
            Destination destination = destinationSO[0];

            vehicleDestination.IsDestination = true;
            destination.Location = vehicleDestination.Location;
            destinationUI.transform.SetParent(vehicleDestination.transform, false);
            destinationUI.GetComponent<DestinationUI>().SetType(destination);
        }

        private Destination GetUnselectedDest()
        {
            return destinationSO.Where(item => !destinations
                          .Any(item2 => item2.ScenePath == item.ScenePath))
                          .FirstOrDefault();
        }

        private void Shuffle()
        {
            int n = destinations.Count;
            while (n > 1)
            {
                n--;

                int k = Random.Range(0, n + 1);
                Destination value = destinations[k];
                destinations[k] = destinations[n];
                destinations[n] = value;
            }
        }

        internal void ReachedDestination(HexCoordinates location)
        {
            
            Destination destination = destinations.Find(item => item.Location.X == location.X
                                                            && item.Location.Y == location.Y);
            foreach (var item in destinations)
            {
                Debug.Log(item.name);
                Debug.Log(item.Location);
                Debug.Log(item.ScenePath);
            }

            Debug.Log(destination.ScenePath);
            SceneManager.LoadScene(destination.ScenePath);
        }
    }
}
