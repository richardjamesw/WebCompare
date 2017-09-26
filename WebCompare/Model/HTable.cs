using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCompare.Model
{
   public class HTable<K, V>
   {
	   private int TAB_SIZE = 64;
       private int count;
       private HEntry<K,V>[] tab;
	   
	   // Entry class
	   static class HEntry<K, V>
	   {
		   K key;
		   V value;
		   HEntry<K, V> next;
		   
		   // Entry constructor
		   public HEntry(K key, V value)
		   {
			   this.key = key;
               this.value = value;
               this.next = null;
		   }
	   }
	   
	   // Table constructor
	   HTable()
	   {
		   this.count = 0;
           tab = new HEntry[TAB_SIZE];
	   }
	   
	   // Add a value to the hash table
	   public void Put(K key, V value)
	   {
		   // Hash string
		   int h = Match.abs(key.hashCode() % TAB_SIZE);
		   // Create new entry
		   HEntry<K,V> entry = new HEntry<K, V>(key, value);
		   
		   // Insert entry to hash array
		   if (tab[h] == null)
		   {
			   // No collision, insert entry
			   tab[h] = entry;
			   ++count; 
		   }
		   else
		   {
			   // Detected collision, step through list
			   HEntry<K,V> current = tab[h];
			   while (current.next != null)
			   {
				   /* // Check keys
				   if (current.key.Equals(entry.key))
				   {
					   // Replace current value
					   current.value = entry.value;
					   return;
				   } */
				   current = current.next;
			   }
			   // Next is null, insert node
			   current.next = entry;
			   ++count;
		   }
	   }   // End Put
	   
	   // Find a value using a key
	   public V FindVal(K key)
	   {
		   HEntry<K, V>[] temp = tab;
		   int h = Math.abs(key.hashCode() % TAB_SIZE);
		   if (temp[h] == null)
		   {
			   return null;
		   }
		   else
		   {
			   HEntry<K,V> entry = temp[h];   // Hold slot
			   while (entry != null && !entry.key.Equals(key))   // Find matching key
			   {
				   entry = entry.next;
			   }
			   if (entry == null)
			   {
				   return null;   // If key not found return null
			   }
			   else
			   {
				   return entry.value;   // If key is found return value
			   }
		   }
	   } // End FindVal
   }
}








