using System;


public class CustomArray<T>
{
    private T[] _array;     // The internal array that stores items
    private int _count;     

    // Constructor initializes the array with a default size
    public CustomArray(int initialSize = 4)
    {
        _array = new T[initialSize];
        _count = 0;
    }

    // Adds a new item to the array, resizing if full
    public void Add(T item)
    {
        if (_count == _array.Length)
        {
            Resize(_array.Length * 2); 
        }

        _array[_count++] = item; // Add item and increase count
    }

    // Removes the first match of the item from the array
    public bool Remove(T item)
    {
        // Find the item's index in the array
        int index = Array.IndexOf(_array, item, 0, _count);
        if (index == -1) return false;
        
         // Item not found
        //_array is the array being searched.
        //item is the thing you want to find. 
        //0 means start searching from the beginning.
        //_count means only search the used part of the array (ignore empty slots).



        // Shift all items after the found one to the left
        for (int i = index; i < _count - 1; i++)
        {
            _array[i] = _array[i + 1];
        }

        _array[--_count] = default(T); // Clear last item and reduce count
        return true;
    }

    // Searches the array using a condition and returns the first match
    public T Search(Predicate<T> match)
    {
        for (int i = 0; i < _count; i++)
        {
            if (match(_array[i]))
                return _array[i];
        }

        return default(T); // Return default if no match
    }

    // Resizes the internal array to a new size
    public void Resize(int newSize)
    {
        if (newSize < _count)
        {
            throw new InvalidOperationException("Cannot resize to smaller than current count.");
        }

        T[] newArray = new T[newSize];
        for (int i = 0; i < _count; i++)
        {
            newArray[i] = _array[i];
        }

        _array = newArray; // Replace old array with new one
    }

    // Returns the number of items currently in the array
    public int Count()
    {
        return _count;
    }

    // Displays all items in the array
    public void Display()
    {
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_array[i]);
        }
    }
}
