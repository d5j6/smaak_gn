using NodeCanvas.Framework;
using UnityEngine;
using System.Linq;

namespace NodeCanvas.DialogueTrees{

	///An interface to use for whats being said by a dialogue actor
	public interface IStatement{
		string textKey{get;}
        string text{get;}
		AudioClip audio{get;}
		string meta{get;}
	}

	///Holds data of what's being said usualy by an actor
	[System.Serializable]
	public class Statement : IStatement{

		[SerializeField]
		private string _textKey = string.Empty;
        private string _replacedText = string.Empty;
		[SerializeField]
		private AudioClip _audio;
		[SerializeField]
		private string _meta = string.Empty;

		public string textKey
        {
			get { return _textKey; }
			set { _textKey = value; }
		}

        public string text
        {
            get
            {
                if (_replacedText == string.Empty)
                    return LocalizationManager.Instance.GetText(_textKey);
                else
                    return _replacedText; //For when some paramters have been blackboard replaced.
            }
        }

        public AudioClip audio{
			get {return _audio;}
			set {_audio = value;}
		}

		public string meta{
			get {return _meta;}
			set {_meta = value;}
		}

		//required
		public Statement(){}
		public Statement(string textKey){
			this.textKey = textKey;
		}

		public Statement(string textKey, AudioClip audio){
			this.textKey = textKey;
			this.audio = audio;
		}

		public Statement(string textKey, AudioClip audio, string meta){
			this.textKey = textKey;
			this.audio = audio;
			this.meta = meta;
		}

        public Statement(string textKey, string replacedText, AudioClip audio, string meta)
        {
            this.textKey = textKey;
            this._replacedText = replacedText;
            this.audio = audio;
            this.meta = meta;
        }

        ///Replace the text of the statement found in brackets, with blackboard variables ToString and returns a Statement copy
        public Statement BlackboardReplace(IBlackboard bb){
			var s = text;
			var i = 0;
			while ( (i = s.IndexOf('[', i)) != -1){
				
				var end = s.Substring(i + 1).IndexOf(']');
				var input = s.Substring(i + 1, end); //what's in the brackets
				var output = s.Substring(i, end + 2); //what should be replaced (includes brackets)
				
				object o = null;
				if (bb != null){ //referenced blackboard replace
					var v = bb.GetVariable(input, typeof(object));
					if (v != null){
						o = v.value;
					}
				}

				if (input.Contains("/")){ //global blackboard replace
					var globalBB = GlobalBlackboard.Find( input.Split('/').First() );
					if (globalBB != null){
						var v = globalBB.GetVariable( input.Split('/').Last(), typeof(object) );
						if (v != null){
							o = v.value;
						}
					}
				}

				s = s.Replace(output, o != null? o.ToString() : output);

				i++;
			}

			return new Statement(textKey, s, audio, meta);
		}

		public override string ToString(){
			return text;
		}
	}
}