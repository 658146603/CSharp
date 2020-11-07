using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using MikePhil.Charting.Formatter;
using DialogFragment = Android.Support.V4.App.DialogFragment;

namespace BalanceYourIO.Android
{
    public class ChooseDateFragment: DialogFragment
    {
        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_choose_date, container, false);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
        }
        
        
    }
}