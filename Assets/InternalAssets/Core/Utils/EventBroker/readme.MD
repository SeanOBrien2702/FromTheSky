# Event Broker

## How To Use

### Setup
Any object that wants to send and/or recieve events should have a EventBrokerComponent.  

```csharp
private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
```  

Create an broker event class for the purpose which ever purpose is needed

```csharp
public class GameStateChangeEvent
{
	public GameStateChangeEvent(eGameState inOldState, eGameState inNewState)
	{
		OldState = inOldState;
		NewState = inNewState;
	}
	public eGameState OldState;
	public eGameState NewState;
}
```  

### Recieving events
Create a handler function that accepts a BrokerEvent with type of the event you wish to recieve  

```csharp
private void GameStateChangeHandler(BrokerEvent<GameStateChangeEvent> inEvent)
{
	// TODO: Handle game state changes if any
}
```  

Subscribe to an event you wish to recieve passing in the handler  

```csharp
private void Subscribe()
{
	eventBrokerComponent.Subscribe<GameStateChangeEvent>(GameStateChangeHandler);
}
```  

### Sending events
Publish an event passing in the sender and the event  

```csharp
private void PublishStateChange(eGamestate newState)
{
	eventBrokerComponent.Publish<GameStateChangeEvent>(this, new GameStateChangeEvent(currentGameState.GameState, newState));
}
```  

### Unsubscribe
Remember to unsubscribe when events no longer need to be recived by calling Unsubscribe passing in the hendler

```csharp
private void UnSubscribe()
{
	eventBrokerComponent.Unsubscribe<GameStateChangeEvent>(GameStateChangeHandler);
}
```

