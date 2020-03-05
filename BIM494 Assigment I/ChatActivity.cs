﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using static Android.App.ActionBar;

namespace BIM494_Assigment_I
{
    [Activity(Label = "ChatActivity")]
    public class ChatActivity : AppCompatActivity
    {
        private RecyclerView recyclerView;
        private RecyclerViewAdapter adapter;
        private EditText ChatActivityMessageEditText;
        private ProgressBar pb;
        private Button ChatActivitySendButton;
        private int id;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_chat);
            string name = Intent.Extras.GetString("name");
            id = Intent.Extras.GetInt("id");
            Title = name;
            // Create your application here
            pb = FindViewById<ProgressBar>(Resource.Id.pb);
            pb.Max = 10;
            pb.Progress = 0;
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            ChatActivityMessageEditText = FindViewById<EditText>(Resource.Id.chat_activity_message_editText);
            adapter = new RecyclerViewAdapter(MainActivity.messages[id], name);
            RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(ApplicationContext);
            recyclerView.SetLayoutManager(layoutManager);
            recyclerView.SetItemAnimator(new DefaultItemAnimator());
            recyclerView.SetAdapter(adapter);
            recyclerView.ScrollToPosition(MainActivity.messages[id].Count - 1);
            ChatActivitySendButton = FindViewById<Button>(Resource.Id.ChatActivitySendButton);
            ChatActivitySendButton.Click += OnSendButtonClicked;
        }

        private void OnSendButtonClicked(object sender, EventArgs e)
        {
            if(ChatActivityMessageEditText.Text != "")
            {
                MainActivity.messages[id].Add(ChatActivityMessageEditText.Text);
                ChatActivityMessageEditText.Text = "";
                adapter.NotifyDataSetChanged();
                recyclerView.ScrollToPosition(MainActivity.messages[id].Count - 1);
                RunOnUiThread(() =>
                {
                    pb.IncrementProgressBy(1);
                    if (pb.Progress >= 10)
                    {
                        Toast.MakeText(this, "You've reached the message limit.", ToastLength.Long).Show();
                        ChatActivitySendButton.Clickable = false;
                    }
                });
            }    
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.details_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_details)
            {
                Intent intent = new Intent(ApplicationContext, typeof(UserDetailsActivity));
                intent.PutExtra("id", id);
                StartActivity(intent);
            }


            return base.OnOptionsItemSelected(item);
        }


    }
}