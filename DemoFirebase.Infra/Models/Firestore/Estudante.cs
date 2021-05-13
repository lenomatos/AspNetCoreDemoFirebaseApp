using Google.Cloud.Firestore;
using System.Collections.Generic;

namespace DemoFirebase.Infra.Models.Firestore
{
    [FirestoreData]
    public class Estudante : Base
    {
        [FirestoreProperty]
        public int Matricula { get; set; }
        [FirestoreProperty]
        public string Nome { get; set; }
        [FirestoreProperty]
        public bool Status { get; set; }

        [FirestoreProperty]
        public List<string> Livros { get; set; }
    }
}
