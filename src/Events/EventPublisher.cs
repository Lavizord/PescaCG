using System;
using System.Collections.Generic;

//! Never acess the sender when declaring the event function.
//  private void OnCardPressed()
//  {
//        GD.Print("Card was Pressed!");
//        EventRegistry.GetEventPublisher("OnCardPressed").RaiseEvent(this);
//  }
public delegate void EventHandler<T>(object sender, T args);

public class EventPublisher<T>
{
    public event EventHandler<T> MyEvent;
    
    // Dont use this direcly to raise the Event, do something like: EventRegistry.GetEventPublisher("OnCardPressed").RaiseEvent(this);
    public void RaiseEvent(T args)
    {
        MyEvent?.Invoke(this, args);
    }
}
