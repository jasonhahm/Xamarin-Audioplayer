using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using AudioPlayer.Services;
using Xamarin.Forms;

namespace AudioPlayer.ViewModels
{
	public class AudioPlayerViewModel: INotifyPropertyChanged
	{
        List<string> playlist = new List<string>
            { "Galway.mp3", "Bank.mp3", "Air.mp3" };

        int index = 0;

        private IAudioPlayerService _audioPlayer;
		private bool _isStopped;
		public event PropertyChangedEventHandler PropertyChanged;

		public AudioPlayerViewModel(IAudioPlayerService audioPlayer)
		{
			_audioPlayer = audioPlayer;
			_audioPlayer.OnFinishedPlaying = () => {
				_isStopped = true;
				CommandText = "Play";
			};
			CommandText = "Play";
            FileText = playlist[index];
			_isStopped = true;
		}

		private string _commandText;
		public string CommandText
		{
			get { return _commandText;}
			set
			{
				_commandText = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CommandText"));
			}
		}

        private string _fileText;
        public string FileText
        {
            get { return _fileText; }
            set
            {
                _fileText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FileText"));
            }
        }

        private ICommand _playPauseCommand;
		public ICommand PlayPauseCommand
		{
			get
			{
				return _playPauseCommand ?? (_playPauseCommand = new Command(
					(obj) => 
				{
					if (CommandText == "Play")
					{
						if (_isStopped)
						{
							_isStopped = false;
							_audioPlayer.Play(playlist[index]);
						}
						else
						{
							_audioPlayer.Play();
						}
						CommandText = "Pause";
					}
					else
					{
						_audioPlayer.Pause();
						CommandText = "Play";
					}
				}));
			}
		}

        private ICommand _PrevCommand;
        public ICommand PrevCommand
        {
            get
            {
                return _PrevCommand ?? (_PrevCommand = new Command(
                    (obj) =>
                    {
                         if (index>0)
                         {
                            index = index - 1;
                            _audioPlayer.Play(playlist[index]);
                            FileText = playlist[index];
                        } else
                         {
                                    
                         } 
                    }));
            }
        }

        private ICommand _NextCommand;
        public ICommand NextCommand
        {
            get
            {
                return _NextCommand ?? (_NextCommand = new Command(
                    (obj) =>
                    {
                        if (index < playlist.Count -1 )
                        {
                            index = index + 1;
                            _audioPlayer.Play(playlist[index]);
                            FileText = playlist[index];
                        }
                        else
                        {

                        }
                    }));
            }
        }
    }
}
