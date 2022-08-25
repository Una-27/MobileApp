using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ModelLibrary;
using Plugin.Media;
using SkiaSharp;
using SkiaSharp.Views.Android;

namespace Calcul
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {//Загрузка
        public class SplashActivity : AppCompatActivity
        {
            static readonly string TAG = "X:" + typeof(SplashActivity).Name;

            public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
            {
                base.OnCreate(savedInstanceState, persistentState);
                Android.Util.Log.Debug(TAG, "SplashActivity.OnCreate");
            }
            protected override void OnResume()
            {
                base.OnResume();
                Task startupWork = new Task(() => { SimulateStartup(); });
                startupWork.Start();
            }
            async void SimulateStartup()
            {
                Android.Util.Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
                await Task.Delay(5); // Simulate a bit of startup work.
                Android.Util.Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }
        }
        DrawerLayout drawer;
        NavigationView navigationView;
        RelativeLayout basis;
        PersonalData user;
        MenuData week;
        List<FrameLayout> interface_fl;
        Color main_; 
        int main_height_, main_width_;
        bool flagexit = false;

        Android.Support.V7.Widget.Toolbar toolbar;
        //menu_callories.SetOnTouchListener(new OnSwipeTouchListener(menu_callories.Context));
        public FrameLayout Create_framelayout(int w_, int h_, float t_x, float t_y, Android.Graphics.Color color)
        {
            FrameLayout framelayout = new FrameLayout(drawer.Context);
            framelayout.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            framelayout.TranslationX = t_x;
            framelayout.TranslationY = t_y;
            framelayout.SetBackgroundColor(color);
            return framelayout;
        }
        public Button Create_button(int w_, int h_, float t_x, float t_y, string t_, GravityFlags flag, TypefaceStyle style, int size)
        {
            Button button = new Button(drawer.Context);
            button.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            button.TranslationX = t_x;
            button.TranslationY = t_y;
            button.Text = t_;
            button.SetTextColor(Android.Graphics.Color.Black);
            button.Gravity = flag;
            button.TextSize = size;
            button.SetTypeface(Typeface.SansSerif, style);
            //button.SetBackgroundColor(Android.Graphics.Color.Indigo);
            return button;
        }
        public ImageView Create_imageview(int w_, int h_, float t_x, float t_y)
        {
            ImageView imageview = new ImageView(drawer.Context);
            imageview.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            imageview.TranslationX = t_x;
            imageview.TranslationY = t_y;
            imageview.Click += (s_, e_) => { };
            //button.SetBackgroundColor(Android.Graphics.Color.Indigo);
            return imageview;
        }
        public TextView Create_textview(int w_, int h_, float t_x, float t_y, string t_, GravityFlags flag, TypefaceStyle style, int size)
        {
            TextView textview = new TextView(drawer.Context);
            textview.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            textview.Text = t_;
            textview.TranslationX = t_x;
            textview.TranslationY = t_y;
            textview.SetTextColor(Android.Graphics.Color.Black);
            textview.Gravity = flag;
            textview.TextSize = size;
            textview.SetTypeface(Typeface.SansSerif, style);
            //textview.SetBackgroundColor(Android.Graphics.Color.Indigo);
            return textview;
        }
        public EditText Create_edittext(int w_, int h_, float t_x, float t_y, string t_, GravityFlags flag, TypefaceStyle style, int size)
        {
            EditText edittext = new EditText(drawer.Context);
            edittext.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            edittext.Text = t_;
            edittext.TranslationX = t_x;
            edittext.TranslationY = t_y;
            edittext.SetTextColor(Android.Graphics.Color.Black);
            edittext.Gravity = flag;
            edittext.TextSize = size;
            edittext.SetTypeface(Typeface.SansSerif, style);
            //edittext.SetBackgroundColor(Android.Graphics.Color.Indigo);
            return edittext;
        }
        public Spinner Create_spinner(int w_, int h_, string[] t_, float t_x, float t_y)
        {
            Spinner spinner = new Spinner(drawer.Context);
            spinner.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            spinner.TranslationX = t_x;
            spinner.TranslationY = t_y;
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, t_);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            return spinner;
        }
        public NumberPicker Create_numberpicker(int w_, int h_, int[] t_, float t_x, float t_y)
        {
            NumberPicker numberpicker = new NumberPicker(drawer.Context);
            numberpicker.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            numberpicker.Value = t_[0];
            numberpicker.MinValue = t_[1];
            numberpicker.MaxValue = t_[2];
            numberpicker.TranslationX = t_x;
            numberpicker.TranslationY = t_y;
            return numberpicker;
        }
        public DatePicker Create_datepicker(int w_, int h_, float t_x, float t_y)
        {
            DatePicker datepicker = new DatePicker(drawer.Context);
            datepicker.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            datepicker.TranslationX = t_x;
            datepicker.TranslationY = t_y;
            //calendarview.MaxDate = t_[0];
            //calendarview.MinDate = t_[1];
            return datepicker;
        }
        public SKCanvasView Create_canvasview(int w_, int h_, float t_x, float t_y)
        {
            double k = user.target_weight - user.current_weight; //цель - текущее
            SKColor background = new SKColor(0xFD, 0xF4, 0xE3); //#FDF4E3
            SKColor graf = new SKColor(0xF3, 0xC3, 0x30); //#f3c330

            var canvasView = new SKCanvasView(basis.Context);
            canvasView.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            canvasView.TranslationX = t_x;
            canvasView.TranslationY = t_y;
            canvasView.PaintSurface += (s_, e_) =>
            {
                float a = 25;
                float b = h_ - 50;
                float d = (b - a);
                float c = (b + a) / 2;
                // получаем текущую поверхность из аргументов
                var surface = e_.Surface;
                // Получаем холст на котором будет рисовать
                var canvas = surface.Canvas;
                // Создаем основу пути
                var pathStroke = new SKPaint
                {
                    IsAntialias = true,
                    Style = SKPaintStyle.Stroke,
                    Color = graf,
                    StrokeWidth = 10
                };
                // Создаем путь
                var path = new SKPath();
                if (k < 0) k = a; else if (k > 0) k = b; else k = (a + b) / 2;
                SKPoint x1 = new SKPoint(w_ / 4 - 25, d);
                SKPoint x2 = new SKPoint(w_ / 2 - 25, c);
                SKPoint x3 = new SKPoint(w_ - 50, b - (float)k + 25);
                path.MoveTo(25, (float)k);
                path.CubicTo(x1, x2, x3);
                canvas.DrawCircle(25, (float)k, 5, pathStroke);
                canvas.DrawCircle(w_ - 50, b - (float)k + 25, 5, pathStroke);
                // Рисуем путь
                canvas.DrawPath(path, pathStroke);
            };
            return canvasView;
        }

        private async Task AnimationObjectAsync(FrameLayout s_, int h, bool f)
        {
            if (f)
            {
                s_.Visibility = ViewStates.Visible;
                for (int i = 1; i < h; i += 8)
                {
                    s_.LayoutParameters = new FrameLayout.LayoutParams(s_.Width, i);
                    await Task.Delay(1);
                }
            }
            else
            {
                for (int i = h; i > 1; i -= 8)
                {
                    s_.LayoutParameters = new FrameLayout.LayoutParams(s_.Width, i);
                    await Task.Delay(1);
                }
                s_.Visibility = ViewStates.Invisible;
            }
        }

        private async Task ExitFlagNot()
        {
            View view = (View)basis;
            Snackbar.Make(view, "Для выхода из приложения нажмите повторно.", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            await Task.Delay(10000);
            flagexit = false;
        }

        //Профильный framelayout
        public void Create_ProfileFramelayout()
        {
            ImageView image_navigation = FindViewById<ImageView>(Resource.Id.imageView);
            image_navigation.LayoutParameters = new LinearLayout.LayoutParams(300, 300);
            if (user.image != null) image_navigation.SetImageBitmap(user.Convert_image_to(user.image));
            else image_navigation.SetImageResource(Resource.Drawable.li);
            TextView text_navigation = FindViewById<TextView>(Resource.Id.texttView);
            text_navigation.TranslationX = 50;
            text_navigation.TextSize = 20;
            text_navigation.SetTextColor(Color.Black);
            text_navigation.Text = user.name;
            user.FileWR(user);
            toolbar.RemoveAllViews();
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();
            TextView prof = Create_textview(toolbar.Width, toolbar.Height, 0, 0,
                "Профиль", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 20);
            toolbar.AddView(prof);
            toolbar.SetBackgroundColor(main_);
            basis.RemoveAllViews();


            FrameLayout profile = Create_framelayout(main_width_, main_height_, 0, 0, Android.Graphics.Color.White);
            profile.RemoveAllViews();
            profile.VerticalScrollBarEnabled = true;
            /*ScrollView profile_scrol = new ScrollView(profile.Context);
            profile_scrol.LayoutParameters = new LinearLayout.LayoutParams(main_width_, main_height_);
            profile_scrol.VerticalScrollBarEnabled = true;
            profile_scrol.ScrollY = 0;*/
            //profile_scrol.AddView(profile);
            ImageView im__ = Create_imageview(main_width_, main_width_, main_width_ - (main_width_ * 2 / 3), (main_height_) - (main_width_ * 2 / 3));
            im__.SetImageResource(Resource.Drawable.s);
            profile.AddView(im__);

            FrameLayout user_name = Create_framelayout(main_width_ - 100, main_height_ / 5, 50, 50, main_);
            user_name.RemoveAllViews();
            user_name.SetBackgroundResource(Resource.Drawable.fram_main1);
            TextView user_name_textview = Create_textview((main_width_ * 2) / 3, main_height_ / 5, (main_width_) / 3 + 50, 0,
                user.name.ToUpper(), GravityFlags.CenterVertical, TypefaceStyle.Bold, 25);
            user_name.AddView(user_name_textview);
            ImageView user_image = Create_imageview((main_width_) / 3, main_height_ / 5 - 20, 10, 10);
            if (user.image == null) user_image.SetImageResource(Resource.Drawable.li);
            else user_image.SetImageBitmap(user.Convert_image_to(user.image));
            user_image.Click += async (s__, e__) =>
            {
                try
                {
                    await CrossMedia.Current.Initialize();
                    var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                        CompressionQuality = 40

                    });
                    if (file != null)
                    {
                        byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
                        user_image.SetImageBitmap(BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length));
                        user.image = imageArray;
                    }
                else if (user.image == null) user_image.SetImageResource(Resource.Drawable.design_ic_visibility_off);
                else user_image.SetImageBitmap(user.Convert_image_to(user.image));
                Create_ProfileFramelayout();
                }
                catch { }
            };
            user_name.AddView(user_image);
            TextView setting_text = Create_textview(150, 150, main_width_ - 250, 0,
                 "⚙", GravityFlags.Center, TypefaceStyle.Bold, 45);
            setting_text.Click += (s_, e_) => { Create_SettingFramelayout(); };
            user_name.AddView(setting_text);
            Button statistics_button = Create_button(main_width_ / 2, 250, main_width_ / 4, main_height_ / 5 + 100, "Статистика", GravityFlags.Center, TypefaceStyle.Bold, 15);
            //statistics_button.SetBackgroundResource(Resource.Drawable.fram_add1);
            statistics_button.Click += (s__, e__) => 
            {
                basis.RemoveAllViews();

                FrameLayout rozrahunok_framelayout = Create_framelayout(main_width_, main_height_, 0, 0, Color.White);
                toolbar.RemoveAllViews();
                Button back = Create_button(toolbar.Height, toolbar.Height, -(int)(toolbar.Height * 1.25), 0, "✖", GravityFlags.Center, TypefaceStyle.Bold, 20);
                back.SetBackgroundDrawable(toolbar.Background);
                back.Click += (s, e) => { basis.RemoveAllViews(); Create_ProfileFramelayout(); };
                toolbar.AddView(back);

                FrameLayout[] roz_mass = new FrameLayout[3];
                int h_roz = (main_height_ - 100) / 3;
                string[] roz_mass_head = new string[3] { "Ваш индекс массы тела", "Идеальные параметры тела", "Тип телосложения" };
                for (int i = 0; i < 3; i++)
                {
                    roz_mass[i] = Create_framelayout(main_width_ - 50, h_roz - 25, 25, 50 + (i * h_roz), Color.White);
                    roz_mass[i].SetBackgroundResource(Resource.Drawable.fram_add1);
                    TextView roz_mass_head_text = Create_textview(main_width_ - 50, 125, 0, 0,
                         roz_mass_head[i], GravityFlags.Center, TypefaceStyle.Italic, 25);
                    roz_mass[i].AddView(roz_mass_head_text);
                    rozrahunok_framelayout.AddView(roz_mass[i]);
                }
                {
                    int imit_int = (int)((double)user.current_weight / (double)(((double)(user.height) / 100) * ((double)(user.height) / 100)));//(user.current_weight / ((float)(user.height / 100) * (float)(user.height / 100)));
                    TextView imitint_text = Create_textview(main_width_ - 50, 250, 25, 100,
                         imit_int.ToString() + ".0", GravityFlags.CenterVertical, TypefaceStyle.Bold, 50);
                    roz_mass[0].AddView(imitint_text);
                    TextView imitint_text_add = Create_textview(main_width_ - 50, 250, (main_width_ - 50) / 3, 100,
                        "", GravityFlags.CenterVertical, TypefaceStyle.Normal, 15);
                    if ((imit_int < 18) & (imit_int >= 15))
                    {
                        imitint_text_add.Text = "Дефицит массы тела";
                        imitint_text_add.SetTextColor(Color.Rgb(25, 25, 112));
                    }
                    else if ((imit_int < 25) & (imit_int >= 18))
                    {
                        imitint_text_add.Text = "Норма";
                        imitint_text_add.SetTextColor(Color.Rgb(0, 100, 0));
                    }
                    else if ((imit_int < 30) & (imit_int >= 25))
                    {
                        imitint_text_add.Text = "Лишний вес";
                        imitint_text_add.SetTextColor(Color.Rgb(255, 165, 0));
                    }
                    else if ((imit_int < 40) & (imit_int >= 30))
                    {
                        imitint_text_add.Text = "Ожирение";
                        imitint_text_add.SetTextColor(Color.Rgb(128, 0, 0));
                    }
                    roz_mass[0].AddView(imitint_text_add);
                    ImageView imitint_image = Create_imageview(main_width_, h_roz - 450, 0, 350);
                    imitint_image.SetImageResource(Resource.Drawable.imit);
                    roz_mass[0].AddView(imitint_image);
                }
                {
                    int imt_h = (h_roz - 125) / 3;
                    string[] imit_int = new string[3];
                    string[] imit_head = new string[3] { "Вес", "Обхват талии", "Обхват бедер" };
                    imit_int[0] = ((int)((double)20 * (double)(((double)(user.height) / 100) * ((double)(user.height) / 100)))).ToString() + " кг";
                    imit_int[1] = ((int)((double)0.47 * (double)(double)(user.height))).ToString() + " см";
                    imit_int[2] = ((int)((double)0.62 * (double)(double)(user.height))).ToString() + " см";
                    for (int j = 0; j < 3; j++)
                    {
                        TextView imitint_text1 = Create_textview(main_width_ - 50, imt_h - 25, 25, 100 + (j * imt_h),
                             imit_int[j].ToString(), GravityFlags.CenterVertical, TypefaceStyle.Bold, 40);
                        roz_mass[1].AddView(imitint_text1);
                        TextView imitint_text_add1 = Create_textview(main_width_ - 50, imt_h - 25, (main_width_ - 50) / 3 + 50, 100 + (j * imt_h),
                            imit_head[j], GravityFlags.CenterVertical, TypefaceStyle.Normal, 15);
                        roz_mass[1].AddView(imitint_text_add1);
                    }
                    ImageView imitint_image = Create_imageview(main_width_ / 3, h_roz + 55, main_width_ * 2 / 3 - 50, 0);
                    imitint_image.SetImageResource(Resource.Drawable.ideal);
                    roz_mass[1].AddView(imitint_image);
                }
                {
                    int imit_int = 0;
                    TextView imitint_text = Create_textview(main_width_ - 50, 150, (main_width_ - 50) / 2 + 25, 150,
                         "Запястье: " + imit_int.ToString() + " см", GravityFlags.CenterVertical, TypefaceStyle.Bold, 20);
                    roz_mass[2].AddView(imitint_text);

                    TextView imitint_text_ = Create_textview(main_width_ - 50, h_roz - 325, 0, 300,
                         "", GravityFlags.Center, TypefaceStyle.Bold, 40);
                    roz_mass[2].AddView(imitint_text_);
                    if (user.gender == false)
                    {
                        if (imit_int < 16) imitint_text_.Text = "Анастенический";
                        else if ((imit_int >= 16) & (imit_int < 18)) imitint_text_.Text = "Нормостенический";
                        else if (imit_int >= 18) imitint_text_.Text = "Гиперстенический";
                    }
                    else
                    {
                        if (imit_int < 14) imitint_text_.Text = "Анастенический";
                        else if ((imit_int >= 14) & (imit_int < 16)) imitint_text_.Text = "Нормостенический";
                        else if (imit_int >= 16) imitint_text_.Text = "Гиперстенический";
                    }
                    FrameLayout add_ = Create_framelayout(main_width_ - 100, (main_width_ - 100) / 2 + 15, 50, (main_height_ - (main_width_ - 100)) / 2, Color.White);
                    add_.SetBackgroundResource(Resource.Drawable.fram_main1);
                    add_.Visibility = ViewStates.Invisible;
                    Button age_button = Create_button((main_width_ - 50) / 2 - 50, 150, 25, 150, "Ввести обхват запястья", GravityFlags.Center, TypefaceStyle.Normal, 15);
                    age_button.SetBackgroundResource(Resource.Drawable.fram_main1);
                    age_button.Click += (s, e) =>
                    {
                        int k = 0;
                        add_.RemoveAllViews();
                        TextView h_ = Create_textview(main_width_ - 100, 100, 0, 10,
                            "Обхват запястья", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                        EditText text = Create_edittext(main_width_ - 350, (main_width_ - 100) / 2 - 150, 125, 50,
                            k.ToString(), GravityFlags.Center, TypefaceStyle.Bold, 50);
                        text.InputType = Android.Text.InputTypes.ClassNumber;
                        text.TextChanged += (s_, e_) =>
                        {
                            if (text.Length() > 2) text.Text = text.Text.Remove(text.Text.Length - 1, 1);
                            text.SetSelection(text.Text.Length);
                            if (text.Text != "") k = Convert.ToInt32(text.Text); else k = 0;
                        };
                        Button plus = Create_button(100, 100, 50, ((main_width_ - 100) / 2 - 100) / 2 - 50,
                            "-", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                        plus.SetBackgroundResource(Resource.Drawable.draver_fram);
                        plus.Click += (s_, e_) =>
                        {
                            if (k > 0) k -= 1;
                            text.Text = k.ToString();
                        };
                        Button minus = Create_button(100, 100, (main_width_ - 250), ((main_width_ - 100) / 2 - 100) / 2 - 50,
                            "+", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                        minus.SetBackgroundResource(Resource.Drawable.draver_fram);
                        minus.Click += (s_, e_) =>
                        {
                            k++;
                            text.Text = k.ToString();
                        };
                        Button save = Create_button(main_width_ - 200, 150, 50, ((main_width_ - 100) / 2) - 175,
                            "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                        save.Click += (s_, e_) =>
                        {
                            imit_int = k;
                            imitint_text.Text = "Запястье: " + imit_int.ToString() + " см";
                            add_.Visibility = ViewStates.Invisible;
                            text.Enabled = false;
                            if (user.gender == false)
                            {
                                if (imit_int < 16) imitint_text_.Text = "Анастенический";
                                else if ((imit_int >= 16) & (imit_int < 18)) imitint_text_.Text = "Нормостенический";
                                else if (imit_int >= 18) imitint_text_.Text = "Гиперстенический";
                            }
                            else
                            {
                                if (imit_int < 14) imitint_text_.Text = "Анастенический";
                                else if ((imit_int >= 14) & (imit_int < 16)) imitint_text_.Text = "Нормостенический";
                                else if (imit_int >= 16) imitint_text_.Text = "Гиперстенический";
                            }

                        };
                        add_.AddView(plus);
                        add_.AddView(minus);
                        add_.AddView(save);
                        add_.AddView(h_);
                        add_.AddView(text);
                        add_.Visibility = ViewStates.Visible;
                    };

                    roz_mass[2].AddView(age_button);
                    rozrahunok_framelayout.AddView(add_);

                }
                basis.AddView(rozrahunok_framelayout);
            };

            //Button history_button = Create_button(main_width_ / 2, 250, main_width_ / 2, main_height_ / 5 + 100, "История", GravityFlags.Center, TypefaceStyle.Bold, 15);
            //history_button.Click += (s__, e__) => { };

            FrameLayout user_grafic = Create_framelayout(main_width_ - 100, main_height_ / 5 + 100, 50, main_height_ / 5 + 400, Android.Graphics.Color.DarkOrange);
            user_grafic.SetBackgroundResource(Resource.Drawable.fram_add1);
            user_grafic.RemoveAllViews();
            SKCanvasView graficview = Create_canvasview(main_width_ - 200, main_height_ / 5 - 100, 100, 75);
            user_grafic.AddView(graficview);
            {
                TextView head_ = Create_textview(main_width_ - 100, 75, 0, 0,
                    "Ц Е Л Ь", GravityFlags.Center, TypefaceStyle.Bold, 20);
                user_grafic.AddView(head_);
            }
            {
                TextView head_ = Create_textview(main_width_ - 100, 75, 0, main_height_ / 5,
                    "> >   " + user.Convert_purpose().ToUpper() + "   < <", GravityFlags.Center, TypefaceStyle.BoldItalic, 15);
                user_grafic.AddView(head_);
            }
            for (int i = 0; i < 2; i++)
            {
                double[] k = new double[3] { user.current_weight, user.target_weight, user.target_weight - user.current_weight };
                if (k[2] < 0) k[2] = 75; else if (k[2] > 0) k[2] = main_height_ / 5 - 100; else k[2] = (main_height_ / 5 - 25) / 2;
                if (i != 0) k[2] = main_height_ / 5 - 100 - k[2] + 75;
                TextView a = Create_textview(main_width_ - 100, 75, 10, (float)k[2],
                    k[i].ToString(), GravityFlags.Left, TypefaceStyle.Italic, 15);
                user_grafic.AddView(a);
            }

            {
                TextView head_ = Create_textview(main_width_, 75, 0, ((main_height_ / 5) * 2) + 550,
                      "Л И Ч Н Ы Е   Д А Н Н Ы Е", GravityFlags.Center, TypefaceStyle.Bold, 20);
                profile.AddView(head_);
            }
            {
                string s1 = $"\nВОЗРАСТ\n\nПОЛ\n\nРОСТ\n\nТЕКУЩИЙ ВЕС\n\nУРОВЕНЬ АКТИВНОСТИ\n\nНОРМА КАЛОРИЙ";
                string s2 = $"\n{user.age.ToString()}\n\n{user.Convert_gender()}" +
                    $"\n\n{user.height.ToString()}\n\n{user.current_weight.ToString()}" +
                    $"\n\n{user.Convert_activity()}\n\n{user.calories.ToString()}";
                TextView head1_ = Create_textview(main_width_ - 20, 2 * (main_height_ / 5), 10, ((main_height_ / 5) * 2) + 625,
                      s1, GravityFlags.Left, TypefaceStyle.Normal, 15);
                profile.AddView(head1_);
                TextView head2_ = Create_textview(main_width_ - 20, 2 * (main_height_ / 5), (main_width_ / 2) + 10, ((main_height_ / 5) * 2) + 625,
                      s2, GravityFlags.Left, TypefaceStyle.Italic, 15);
                profile.AddView(head2_);
            }

            //profile.AddView(history_button);
            profile.AddView(statistics_button);
            profile.AddView(user_name);
            profile.AddView(user_grafic);
            basis.AddView(profile);
        }
        //Настройки профиля framelayout
        private void Create_SettingFramelayout()
        {
            if (user.purpose == 0) if (user.target_weight >= user.current_weight) user.target_weight = user.current_weight - 1;
            if (user.purpose == 1) user.target_weight = user.current_weight;
            if (user.purpose == 2) if (user.target_weight <= user.current_weight) user.target_weight = user.current_weight + 1;
            FrameLayout add_ = Create_framelayout(main_width_ - 100, (main_width_ - 100) / 2 + 15, 50, (main_height_ - (main_width_ - 100)) / 2, Color.White);
            add_.SetBackgroundResource(Resource.Drawable.fram_main1);
            add_.Visibility = ViewStates.Invisible;
            FrameLayout setting = Create_framelayout(main_width_, main_height_, 0, 0, Color.White);
            toolbar.RemoveAllViews();
            toolbar.SetBackgroundColor(main_);
            basis.RemoveAllViews();
            Button back = Create_button(toolbar.Height, toolbar.Height, -(int)(toolbar.Height * 1.25), 0, "✖", GravityFlags.Center, TypefaceStyle.Bold, 20);
            back.SetBackgroundDrawable(toolbar.Background);
            back.Click += (s_, e_) =>
            {
                bool fl = false;
                if (user.purpose == 1) { user.target_weight = user.current_weight; fl = true; }
                if (user.purpose == 0) if (user.current_weight > user.target_weight) fl = true;
                if (user.purpose == 2) if (user.current_weight < user.target_weight) fl = true;
                if (fl)
                {
                    basis.RemoveView(setting);
                    Create_ProfileFramelayout();
                }
                else
                {

                    View view = (View)s_;
                    Snackbar.Make(view, "Целевой и текущий вес не соответствуют поставленной цели.", Snackbar.LengthLong)
                        .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
            };
            toolbar.AddView(back);
            Button fon = Create_button(main_width_, main_height_, 0, 0, "", GravityFlags.Center, TypefaceStyle.Normal, 0);
            fon.SetBackgroundDrawable(setting.Background);
            fon.Click += (s__, e__) =>
            {
                add_.Visibility = ViewStates.Invisible;
                fon.Visibility = ViewStates.Invisible;
                Create_SettingFramelayout();
            };
            fon.Visibility = ViewStates.Invisible;
            TextView prof = Create_textview(toolbar.Width, toolbar.Height, -(int)(toolbar.Height * 1.25), 0,
                "Настройки", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 20);
            toolbar.AddView(prof);


            TextView head_ = Create_textview(main_width_, 100, 0, 10,
                  "Ц Е Л Ь", GravityFlags.Center, TypefaceStyle.Bold, 20);
            setting.AddView(head_);


            FrameLayout target_button = Create_framelayout(main_width_ - 50, 150, 25, 417, Color.White);
            target_button.SetBackgroundResource(Resource.Drawable.fram_add1);
            TextView target_button_h1 = Create_textview(main_width_ - 50, 150, 10, 0,
                "Ц е л е в о й   в е с", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 17);
            TextView target_button_h2 = Create_textview(main_width_ - 50, 150, -10, 0,
                user.target_weight.ToString(), GravityFlags.Right | GravityFlags.CenterVertical, TypefaceStyle.Italic, 15);
            target_button.AddView(target_button_h1);
            target_button.AddView(target_button_h2);
            if (user.purpose == 1) target_button.Visibility = ViewStates.Invisible;

            FrameLayout current_button = Create_framelayout(main_width_ - 50, 150, 25, 266, Color.White);
            current_button.SetBackgroundResource(Resource.Drawable.fram_add1);
            TextView current_button_h1 = Create_textview(main_width_ - 50, 150, 10, 0,
                "Т е к у щ и й   в е с", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 17);
            TextView current_button_h2 = Create_textview(main_width_ - 50, 150, -10, 0,
                user.current_weight.ToString(), GravityFlags.Right | GravityFlags.CenterVertical, TypefaceStyle.Italic, 15);
            current_button.AddView(current_button_h1);
            current_button.AddView(current_button_h2);
            current_button.Click += Change_weight;
            target_button.Click += Change_weight;

            void Change_weight(object sender, EventArgs eventArgs)
            {
                fon.Visibility = ViewStates.Visible;
                int k = user.current_weight;
                string s = "Т е к у щ и й   в е с";
                if (sender == target_button)
                {
                    k = user.target_weight;
                    s = "Ц е л е в о й   в е с";
                }
                add_.RemoveAllViews();
                TextView h_ = Create_textview(main_width_ - 100, 100, 0, 10,
                    s, GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                EditText text = Create_edittext(main_width_ - 350, (main_width_ - 100) / 2 - 150, 125, 50,
                    k.ToString(), GravityFlags.Center, TypefaceStyle.Bold, 50);
                text.InputType = Android.Text.InputTypes.ClassNumber;
                text.TextChanged += (s__, e__) => 
                {
                    if (text.Length() > 3) text.Text = text.Text.Remove(text.Text.Length - 1, 1);
                    text.SetSelection(text.Text.Length);
                    if (text.Text != "") k = Convert.ToInt32(text.Text); else k = 0;
                };
                Button plus = Create_button(100, 100, 50, ((main_width_ - 100) / 2 - 100) / 2 - 50,
                    "-", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                plus.SetBackgroundResource(Resource.Drawable.draver_fram);
                plus.Click += (s__, e__) =>
                {
                    k -= 1;
                    if ((sender == target_button) & (user.purpose == 2) & (user.current_weight >= k))
                    {
                        View view = (View)s__;
                        Snackbar.Make(view, "Целевой вес не соответствуют поставленной цели.", Snackbar.LengthLong)
                            .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                        k++;
                    }
                    text.Text = k.ToString();
                };
                Button minus = Create_button(100, 100, (main_width_ - 250), ((main_width_ - 100) / 2 - 100) / 2 - 50,
                    "+", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                minus.SetBackgroundResource(Resource.Drawable.draver_fram);
                minus.Click += (s__, e__) =>
                {
                    k++;
                    if ((sender == target_button) & (user.purpose == 0) & (user.current_weight <= k))
                    {
                        View view = (View)s__;
                        Snackbar.Make(view, "Целевой вес не соответствуют поставленной цели.", Snackbar.LengthLong)
                            .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                        k--;
                    }
                    text.Text = k.ToString();
                };
                Button save = Create_button(main_width_ - 200, 150, 50, ((main_width_ - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) =>
                {
                    if ((k > 300) || (k < 30))
                    {
                        k = 65;
                        text.Enabled = false; 
                        if (sender == current_button) text.Text = user.current_weight.ToString();
                        else text.Text = user.target_weight.ToString();
                        text.Enabled = true;
                        View view = (View)s__;
                        Snackbar.Make(view, "Вес выходит за границы допустимого. Можно вводить число в диапазоне от 30 до 300 кг.", Snackbar.LengthLong)
                            .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                    }
                    else
                    {
                        if (sender == current_button) user.current_weight = k;
                        else user.target_weight = k;
                        add_.Visibility = ViewStates.Invisible;
                        fon.Visibility = ViewStates.Invisible;
                        text.Enabled = false;
                        if (((user.purpose == 2) & (user.current_weight >= user.target_weight)) || ((user.purpose == 0) & (user.current_weight <= user.target_weight)))
                        {
                            View view = (View)s__;
                            Snackbar.Make(view, "Целевой вес не соответствуют поставленной цели.", Snackbar.LengthLong)
                                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                        }
                        Create_SettingFramelayout();
                    }
                };
                add_.AddView(plus);
                add_.AddView(minus);
                add_.AddView(save);
                add_.AddView(h_);
                add_.AddView(text);
                add_.Visibility = ViewStates.Visible;
            }

            FrameLayout purpose_button = Create_framelayout(main_width_ - 50, 150, 25, 115, Color.White);
            purpose_button.SetBackgroundResource(Resource.Drawable.fram_add1);
            TextView purpose_button_h1 = Create_textview(main_width_ - 50, 150, 10, 0,
                "М о я   ц е л ь", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 17);
            TextView purpose_button_h2 = Create_textview(main_width_ - 50, 150, -10, 0,
                user.Convert_purpose(), GravityFlags.Right | GravityFlags.CenterVertical, TypefaceStyle.Italic, 15);
            purpose_button.AddView(purpose_button_h1);
            purpose_button.AddView(purpose_button_h2);
            purpose_button.Click += (s_, e_) =>
            {
                fon.Visibility = ViewStates.Visible;
                add_.RemoveAllViews();
                for (int i = 0; i < 3; i++)
                {
                    string[] st = new string[3] { "сбросить вес", "поддержать вес", "набрать вес" };
                    int k = (main_width_ - 100) / 6 - 15;
                    TypefaceStyle ts = TypefaceStyle.Normal;
                    if (i == user.purpose) ts = TypefaceStyle.Italic;
                    Button bt = Create_button(main_width_ - 200, k, 50, i * k + 15, st[i], GravityFlags.Center, ts, 15);
                    bt.Id = i;
                    bt.SetBackgroundDrawable(purpose_button.Background);
                    if (bt.Id == user.purpose) bt.SetBackgroundColor(main_);
                    bt.Click += (s__, e__) =>
                    {
                        user.purpose = bt.Id;
                        if (user.purpose == 0) user.target_weight = user.current_weight - 1;
                        if (user.purpose == 1) user.target_weight = user.current_weight;
                        if (user.purpose == 2) user.target_weight = user.current_weight + 1;
                        if (user.purpose == 1) target_button.Visibility = ViewStates.Invisible; else target_button.Visibility = ViewStates.Visible;
                        add_.Visibility = ViewStates.Invisible;
                        fon.Visibility = ViewStates.Invisible;
                        Create_SettingFramelayout();
                    };
                    add_.AddView(bt);
                }
                add_.Visibility = ViewStates.Visible;
            };
            head_ = Create_textview(main_width_, 100, 0, 582,
                              "Л И Ч Н Ы Е   Д А Н Н Ы Е", GravityFlags.Center, TypefaceStyle.Bold, 20);
            setting.AddView(head_);

            FrameLayout name_button = Create_framelayout(main_width_ - 50, 150, 25, 687, Color.White);
            name_button.SetBackgroundResource(Resource.Drawable.fram_add1);
            TextView name_button_h1 = Create_textview(main_width_ - 50, 150, 10, 0,
                "И м я", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 17);
            TextView name_button_h2 = Create_textview(main_width_ - 50, 150, -10, 0,
                user.name, GravityFlags.Right | GravityFlags.CenterVertical, TypefaceStyle.Italic, 15);
            name_button.AddView(name_button_h1);
            name_button.AddView(name_button_h2);
            name_button.Click += (s, e) =>
            {
                fon.Visibility = ViewStates.Visible;
                string k = user.name;
                add_.RemoveAllViews();
                TextView h_ = Create_textview(main_width_ - 100, 100, 0, 10,
                    "И м я", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);

                EditText name_ = Create_edittext(main_width_ - 350, (main_width_ - 100) / 2 - 150, 125, 50,
                    k.ToString(), GravityFlags.Center, TypefaceStyle.Bold, 35);
                name_.InputType = Android.Text.InputTypes.ClassText;

                name_.TextChanged += (s__, e__) =>
                {
                    if (name_.Length() > 10) name_.Text = name_.Text.Remove(name_.Text.Length - 1, 1);
                    name_.SetSelection(name_.Text.Length);
                    if (name_.Text != "") k = name_.Text;
                };

                Button save = Create_button(main_width_ - 200, 150, 50, ((main_width_ - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) =>
                {
                    user.name = name_.Text;
                    add_.Visibility = ViewStates.Invisible;
                    name_.Focusable = false;
                    fon.Visibility = ViewStates.Invisible;
                    name_.Enabled = false;
                    Create_SettingFramelayout();
                };
                add_.AddView(save);
                add_.AddView(h_);
                add_.AddView(name_);
                add_.Visibility = ViewStates.Visible;
            };

            FrameLayout genger_button = Create_framelayout(main_width_ - 50, 150, 25, 838, Color.White);
            genger_button.SetBackgroundResource(Resource.Drawable.fram_add1);
            TextView genger_button_h1 = Create_textview(main_width_ - 50, 150, 10, 0,
                "П о л", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 17);
            TextView genger_button_h2 = Create_textview(main_width_ - 50, 150, -10, 0,
                user.Convert_gender(), GravityFlags.Right | GravityFlags.CenterVertical, TypefaceStyle.Italic, 15);
            genger_button.AddView(genger_button_h1);
            genger_button.AddView(genger_button_h2);
            genger_button.Click += (s, e) =>
            {
                fon.Visibility = ViewStates.Visible;
                bool flag = user.gender;
                add_.RemoveAllViews();
                TextView h_ = Create_textview(main_width_ - 100, 100, 0, 10,
                    "П о л", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                Button save = Create_button(main_width_ - 200, 150, 50, ((main_width_ - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) =>
                {
                    user.gender = flag;
                    add_.Visibility = ViewStates.Invisible;
                    fon.Visibility = ViewStates.Invisible;
                    Create_SettingFramelayout();
                };

                Button w_ = Create_button((main_width_ - 100) / 2 - 25, 150, 25, 150,
                    "Ж", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                Button m_ = Create_button((main_width_ - 100) / 2 - 25, 150, (main_width_ - 100) / 2, 150,
                    "М", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                if (flag == false) m_.SetBackgroundColor(main_);
                else w_.SetBackgroundColor(main_);
                w_.Click += Change_gender;
                m_.Click += Change_gender;

                void Change_gender(object sender, EventArgs eventArgs)
                {
                    m_.SetBackgroundDrawable(save.Background);
                    w_.SetBackgroundDrawable(save.Background);
                    if (sender == m_) { m_.SetBackgroundColor(main_); flag = false; }
                    else { w_.SetBackgroundColor(main_); flag = true; }
                }

                add_.AddView(save);
                add_.AddView(h_);
                add_.AddView(w_);
                add_.AddView(m_);
                add_.Visibility = ViewStates.Visible;
            };

            FrameLayout age_button = Create_framelayout(main_width_ - 50, 150, 25, 989, Color.White);
            age_button.SetBackgroundResource(Resource.Drawable.fram_add1);
            TextView age_button_h1 = Create_textview(main_width_ - 50, 150, 10, 0,
                "В о з р а с т", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 17);
            TextView age_button_h2 = Create_textview(main_width_ - 50, 150, -10, 0,
                user.age.ToString(), GravityFlags.Right | GravityFlags.CenterVertical, TypefaceStyle.Italic, 15);
            age_button.AddView(age_button_h1);
            age_button.AddView(age_button_h2);
            age_button.Click += (s, e) =>
            {
                fon.Visibility = ViewStates.Visible;
                int k = user.age;
                add_.RemoveAllViews();
                TextView h_ = Create_textview(main_width_ - 100, 100, 0, 10,
                    "В о з р а с т", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                EditText text = Create_edittext(main_width_ - 350, (main_width_ - 100) / 2 - 150, 125, 50,
                    k.ToString(), GravityFlags.Center, TypefaceStyle.Bold, 50);
                text.InputType = Android.Text.InputTypes.ClassNumber;
                text.TextChanged += (s__, e__) =>
                {
                    if (text.Length() > 2) text.Text = text.Text.Remove(text.Text.Length - 1, 1);
                    text.SetSelection(text.Text.Length);
                    if (text.Text != "") k = Convert.ToInt32(text.Text); else k = 0;
                };
                Button plus = Create_button(100, 100, 50, ((main_width_ - 100) / 2 - 100) / 2 - 50,
                    "-", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                plus.SetBackgroundResource(Resource.Drawable.draver_fram);
                plus.Click += (s__, e__) =>
                {
                    if (k > 0) k -= 1;
                    text.Text = k.ToString();
                };
                Button minus = Create_button(100, 100, (main_width_ - 250), ((main_width_ - 100) / 2 - 100) / 2 - 50,
                    "+", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                minus.SetBackgroundResource(Resource.Drawable.draver_fram);
                minus.Click += (s__, e__) =>
                {
                    k++;
                    text.Text = k.ToString();
                };
                Button save = Create_button(main_width_ - 200, 150, 50, ((main_width_ - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) =>
                {
                    if ((k > 99) || (k < 10))
                    {
                        text.Enabled = false;
                        text.Text = user.age.ToString();
                        text.Enabled = true;
                        View view = (View)s__;
                        Snackbar.Make(view, "Возраст выходит за границы допустимого. Можно вводить число в диапазоне от 10 до 99 лет.", Snackbar.LengthLong)
                            .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                    }
                    else
                    {
                        user.age = k;
                        add_.Visibility = ViewStates.Invisible;
                        fon.Visibility = ViewStates.Invisible;
                        text.Enabled = false;
                        Create_SettingFramelayout();
                    }
                };
                add_.AddView(plus);
                add_.AddView(minus);
                add_.AddView(save);
                add_.AddView(h_);
                add_.AddView(text);
                add_.Visibility = ViewStates.Visible;
            };

            FrameLayout height_button = Create_framelayout(main_width_ - 50, 150, 25, 1140, Color.White);
            height_button.SetBackgroundResource(Resource.Drawable.fram_add1);
            TextView height_button_h1 = Create_textview(main_width_ - 50, 150, 10, 0,
                "Р о с т", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 17);
            TextView height_button_h2 = Create_textview(main_width_ - 50, 150, -10, 0,
                user.height.ToString(), GravityFlags.Right | GravityFlags.CenterVertical, TypefaceStyle.Italic, 15);
            height_button.AddView(height_button_h1);
            height_button.AddView(height_button_h2);
            height_button.Click += (s, e) =>
            {
                fon.Visibility = ViewStates.Visible;
                int k = user.height;
                add_.RemoveAllViews();
                TextView h_ = Create_textview(main_width_ - 100, 100, 0, 10,
                    "Р о с т", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                EditText text = Create_edittext(main_width_ - 350, (main_width_ - 100) / 2 - 150, 125, 50,
                    k.ToString(), GravityFlags.Center, TypefaceStyle.Bold, 50);
                text.InputType = Android.Text.InputTypes.ClassNumber; 
                text.TextChanged += (s__, e__) =>
                {
                    if (text.Length() > 3) text.Text = text.Text.Remove(text.Text.Length - 1, 1);
                    text.SetSelection(text.Text.Length);
                    if (text.Text != "") k = Convert.ToInt32(text.Text); else k = 0;
                };
                Button plus = Create_button(100, 100, 50, ((main_width_ - 100) / 2 - 100) / 2 - 50,
                    "-", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                plus.SetBackgroundResource(Resource.Drawable.draver_fram);
                plus.Click += (s__, e__) =>
                {
                    if (k > 30) k -= 1;
                    text.Text = k.ToString();
                };
                Button minus = Create_button(100, 100, (main_width_ - 250), ((main_width_ - 100) / 2 - 100) / 2 - 50,
                    "+", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                minus.SetBackgroundResource(Resource.Drawable.draver_fram);
                minus.Click += (s__, e__) =>
                {
                    k++;
                    text.Text = k.ToString();
                };
                Button save = Create_button(main_width_ - 200, 150, 50, ((main_width_ - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) =>
                {
                    if ((k > 250) || (k < 50))
                    {
                        text.Enabled = false;
                        text.Text = user.height.ToString();
                        text.Enabled = true;
                        View view = (View)s__;
                        Snackbar.Make(view, "Рост выходит за границы допустимого. Можно вводить число в диапазоне от 50 до 250 см.", Snackbar.LengthLong)
                            .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                    }
                    else
                    {
                        user.height = k;
                        add_.Visibility = ViewStates.Invisible;
                        text.Enabled = false;
                        fon.Visibility = ViewStates.Invisible;
                        Create_SettingFramelayout();
                    }
                };
                add_.AddView(plus);
                add_.AddView(minus);
                add_.AddView(save);
                add_.AddView(h_);
                add_.AddView(text);
                add_.Visibility = ViewStates.Visible;
            };

            FrameLayout activity_button = Create_framelayout(main_width_ - 50, 150, 25, 1291, Color.White);
            activity_button.SetBackgroundResource(Resource.Drawable.fram_add1);
            TextView activity_button_h1 = Create_textview(main_width_ - 50, 150, 10, 0,
                "У р о в е н ь   а к т и в н о с т и", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 17);
            TextView activity_button_h2 = Create_textview(main_width_ - 50, 150, -10, 0,
                user.Convert_activity(), GravityFlags.Right | GravityFlags.CenterVertical, TypefaceStyle.Italic, 15);
            activity_button.AddView(activity_button_h1);
            activity_button.AddView(activity_button_h2);
            activity_button.Click += (s_, e_) =>
            {
                fon.Visibility = ViewStates.Visible;
                add_.RemoveAllViews();
                for (int i = 0; i < 4; i++)
                {
                    string[] st = new string[4] { "низкий", "средний", "высокий", "очень высокий" };
                    int k = (main_width_ - 100) / 8 - 15;
                    TypefaceStyle ts = TypefaceStyle.Normal;
                    if (i == user.activity) ts = TypefaceStyle.Italic;
                    Button bt = Create_button(main_width_ - 200, k, 50, i * k + 15, st[i], GravityFlags.Center, ts, 15);
                    bt.Id = i;
                    bt.SetBackgroundDrawable(purpose_button.Background);
                    if (bt.Id == user.activity) bt.SetBackgroundColor(main_);
                    bt.Click += (s__, e__) =>
                    {
                        bt.SetBackgroundColor(main_);
                        user.activity = bt.Id;
                        add_.Visibility = ViewStates.Invisible;
                        fon.Visibility = ViewStates.Invisible;
                        Create_SettingFramelayout();
                    };
                    add_.AddView(bt);
                }
                add_.Visibility = ViewStates.Visible;
            };

            FrameLayout food_button = Create_framelayout(main_width_ - 50, 150, 25, 1442, Color.White);
            food_button.SetBackgroundResource(Resource.Drawable.fram_add1);
            TextView food_button_h1 = Create_textview(main_width_ - 50, 150, 10, 0,
                "П р е д п о ч т е н и я   в   е д е", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 17);
            TextView food_button_h2 = Create_textview(main_width_ - 50, 150, -10, 0,
                user.food.ToString(),  GravityFlags.Right | GravityFlags.CenterVertical, TypefaceStyle.Italic, 15);
            food_button.AddView(food_button_h1);
            food_button.AddView(food_button_h2);
            food_button.Click += (s_, e_) =>
            {
                fon.Visibility = ViewStates.Visible;
                add_.RemoveAllViews();
                Food_ food_hd;
                for (int i = 0; i < 3; i++)
                {
                    food_hd = (Food_)i;
                    string[] st = new string[3] { "нет", "веган", "вегетарианец" };
                    int k = (main_width_ - 100) / 6 - 15;
                    TypefaceStyle ts = TypefaceStyle.Normal;
                    if (i == (int)user.food) ts = TypefaceStyle.Italic;
                    Button bt = Create_button(main_width_ - 200, k, 50, i * k + 15, st[i], GravityFlags.Center, ts, 15);
                    bt.Id = i;
                    bt.SetBackgroundDrawable(food_button.Background);
                    if (bt.Id == (int)user.food) bt.SetBackgroundColor(main_);
                    bt.Click += (s__, e__) =>
                    {
                        user.food = (Food_)bt.Id;
                        add_.Visibility = ViewStates.Invisible;
                        fon.Visibility = ViewStates.Invisible;
                        Create_SettingFramelayout();
                    };
                    add_.AddView(bt);
                }
                add_.Visibility = ViewStates.Visible;
            };

            basis.AddView(setting);
            basis.AddView(add_);
            setting.AddView(purpose_button); 
            setting.AddView(target_button);
            setting.AddView(current_button);

            setting.AddView(name_button);
            setting.AddView(genger_button);
            setting.AddView(age_button);
            setting.AddView(height_button);
            setting.AddView(activity_button);
            setting.AddView(food_button);
            setting.AddView(fon);
            setting.Visibility = ViewStates.Visible;
            interface_fl.Add(setting);
        }
        //Меню на неделю framelayout
        private void CreateMenuFramelayout(int datadef)
        {
            toolbar.RemoveAllViews();
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();
            TextView prof = Create_textview(toolbar.Width, toolbar.Height, 0, 0,
                "Меню", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 20);
            toolbar.AddView(prof);
            toolbar.SetBackgroundColor(main_);
            basis.RemoveAllViews();

            FrameLayout menuday = Create_framelayout(main_width_, main_height_, 0, 0, Android.Graphics.Color.White);
            menuday.RemoveAllViews();

            FrameLayout menu_fra = Create_framelayout(main_width_ - 100, main_height_ / 3 - 100, 50, 50, main_);
            menu_fra.RemoveAllViews();
            menu_fra.SetBackgroundResource(Resource.Drawable.fram_main1);
            TextView menu_callories = Create_textview((main_width_ - 100), (main_height_ / 3 - 100) / 2, 0, 0,
                user.calories.ToString() + "\nккал", GravityFlags.Center, TypefaceStyle.Bold, 30);
            menu_fra.AddView(menu_callories);
            for (int i = 0; i < 3; i++)
            {
                string[] bzhu_string_h = new string[3] { "БЕЛКИ", "ЖИРЫ", "УГЛЕВОДЫ" };
                int[] bzhu_ = new int[3] { (int)(user.calories * 0.03), (int)(user.calories * 0.02), (int)(user.calories * 0.05) };
                TextView menu_bzhu_h = Create_textview((main_width_ - 100) / 3, (main_height_ / 3 - 100) / 2, i * (main_width_ - 100) / 3, (main_height_ / 3 - 100) / 2,
                    bzhu_string_h[i], GravityFlags.Center, TypefaceStyle.Normal, 17);
                menu_fra.AddView(menu_bzhu_h);
                TextView menu_bzhu = Create_textview((main_width_ - 100) / 3, (main_height_ / 3 - 100) / 2, i * (main_width_ - 100) / 3, (main_height_ / 3 - 100) / 2 + 50,
                    bzhu_[i].ToString() + " г", GravityFlags.Center, TypefaceStyle.Bold, 15);
                menu_fra.AddView(menu_bzhu);
            }
            int dd = 0;
            string[] weekbt = new string[7] { "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ", "ВС" };
            for (int i = 0; i < 7; i++) if (weekbt[i].ToLower() == DateTime.Now.ToString("ddd")) dd = i;
            string date = DateTime.Now.AddDays((double)(datadef - dd)).ToString("dd MMMM, ddd"); //week.menu_of_week[datadef].date;
            TextView head_ = Create_textview(main_width_, 100, 0, main_height_ / 3,
                  date, GravityFlags.Center, TypefaceStyle.Bold, 25);
            menuday.AddView(head_);
            TextView h_ = Create_textview(main_width_ / 2 - 50, 100, 50, main_height_ / 3,
                  "ᐊ", GravityFlags.CenterVertical | GravityFlags.Left, TypefaceStyle.Bold, 25);
            menuday.AddView(h_);
            TextView h = Create_textview(main_width_ / 2 - 50, 100, main_width_ / 2, main_height_ / 3,
                  "ᐅ", GravityFlags.CenterVertical | GravityFlags.Right, TypefaceStyle.Bold, 25);
            menuday.AddView(h);
            h_.Click += Click_h;
            h.Click += Click_h;
            void Click_h(object s, EventArgs e)
            {
                if (s == h) datadef++; else datadef--;
                if ((datadef <= 6) && (datadef >= 0)) CreateMenuFramelayout(datadef);
                else
                {
                    View view = (View)s;
                    Snackbar.Make(view, "На этот день отсутствует меню", Snackbar.LengthLong)
                        .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                    if (datadef > 6) datadef--;
                    if (datadef < 0) datadef++;
                }
            }
            string[] head_but = new string[3] { "З А В Т Р А К", "О Б Е Д", "У Ж И Н" };
            int[] intake_im = new int[3] { Resource.Drawable.b, Resource.Drawable.l, Resource.Drawable.d };
            int[] intake_norm = new int[3] { (int)(user.calories * 0.25), (int)(user.calories * 0.5), (int)(user.calories * 0.25) };
            FrameLayout[] button_intake = new FrameLayout[3];
            for (int i = 0; i < 3; i++)
            {
                int k = (main_height_ - (main_height_ / 3 + 150));
                button_intake[i] = Create_framelayout(main_width_ - 20, k / 3 - 20, 10, (main_height_ / 3 + 150) + (k / 3 * i), Color.Rgb(255, 250, 240));
                button_intake[i].SetBackgroundResource(Resource.Drawable.fram_add1);
                menuday.AddView(button_intake[i]);

                ImageView _image = Create_imageview(k / 3 - 40, k / 3 - 40, 10, 10);
                _image.SetImageResource(intake_im[i]);
                button_intake[i].AddView(_image);

                TextView h_intake = Create_textview((main_width_ - 20) - (k / 3), k / 3 - 20, k / 3, 0,
                      head_but[i], GravityFlags.CenterVertical | GravityFlags.Left, TypefaceStyle.Bold, 25);
                button_intake[i].AddView(h_intake);
                TextView nor_intake = Create_textview((main_width_ - 20) - (k / 3), 50, k / 3, k / 3 - 80,
                      "Рекомендуется: " + intake_norm[i].ToString() + " ккал", GravityFlags.CenterVertical | GravityFlags.Left, TypefaceStyle.Italic, 12);
                button_intake[i].AddView(nor_intake);
                button_intake[i].Click += (s_, e_) =>
                {
                    int id_menu = 0;
                    for (int j = 0; j < 3; j++) if (s_ == button_intake[j]) id_menu = j;
                    CreateAddMenuFramelayot(datadef, id_menu);
                };
            }
            basis.AddView(menuday);
            menuday.AddView(menu_fra);
        }
        public void CreateAddMenuFramelayot(int datadef, int id_menu)
        {
            int dd = 0;
            string[] weekbt = new string[7] { "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ", "ВС" };
            int[] intake_im = new int[3] { Resource.Drawable.b, Resource.Drawable.l, Resource.Drawable.d };
            basis.RemoveAllViews();

            FrameLayout intake_menu = Create_framelayout(main_width_, main_height_, 0, 0, Color.White);

            toolbar.RemoveAllViews();
            Button back = Create_button(toolbar.Height, toolbar.Height, -(int)(toolbar.Height * 1.25), 0, "✖", GravityFlags.Center, TypefaceStyle.Bold, 20);
            back.SetBackgroundDrawable(toolbar.Background);
            back.Click += (s__, e__) => { basis.RemoveAllViews(); CreateMenuFramelayout(datadef); };
            toolbar.AddView(back);


            List<Recipe> mass_intake = new List<Recipe>();
            List<Recipe> mass_intake_week = new List<Recipe>();
            switch (id_menu)
            {
                case 0:
                    mass_intake = week.menu_of_week_add[datadef].BreakfastRecipes;
                    mass_intake_week = week.menu_of_week[datadef].BreakfastRecipes;
                    break;
                case 1:
                    mass_intake = week.menu_of_week_add[datadef].LaunchRecipes;
                    mass_intake_week = week.menu_of_week[datadef].LaunchRecipes;
                    break;
                case 2:
                    mass_intake = week.menu_of_week_add[datadef].DinnerRecipes;
                    mass_intake_week = week.menu_of_week[datadef].DinnerRecipes;
                    break;
            };

            int intake_norm = 0;
            for (int l = 0; l < mass_intake.Count; l++) intake_norm += mass_intake[l].Colories;
                FrameLayout add_intake = Create_framelayout(main_width_ - 100, main_height_ / 3 - 100, 50, 50, main_);
            add_intake.RemoveAllViews();
            add_intake.SetBackgroundResource(Resource.Drawable.fram_main1);
            intake_menu.AddView(add_intake);
            TextView intake_callories = Create_textview((main_width_ - 100) / 2, (main_height_ / 3 - 100) / 2, 0, 0,
                intake_norm.ToString() + "\nккал", GravityFlags.Center, TypefaceStyle.Bold, 30);
            add_intake.AddView(intake_callories);
            int[] bzhu_ = new int[3] { (int)(intake_norm * 0.03), (int)(intake_norm * 0.02), (int)(intake_norm * 0.05) };
            string add_hbzu = "⦿ БЕЛКИ			" + bzhu_[0].ToString() + " г\n⦿ ЖИРЫ			" + bzhu_[1].ToString() + "г \n⦿ УГЛЕВОДЫ			" + bzhu_[2].ToString() + " г";
            TextView menu_bzhu_h = Create_textview((main_width_ - 100) / 2, (main_height_ / 3 - 100) / 2, (main_width_ - 100) / 2, 0,
                add_hbzu, GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Italic, 17);
            add_intake.AddView(menu_bzhu_h);
            string[] head_b_in = new string[3] { "Завтрак", "Обед", "Ужин" };
            int dd_add = 0;
            string[] weekbt_add = new string[7] { "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ", "ВС" };
            for (int j = 0; j < 7; j++) if (weekbt[j].ToLower() == DateTime.Now.ToString("ddd")) dd = j;
            string date_add = DateTime.Now.AddDays((double)(datadef - dd)).ToString("dd MMMM yyyy");
            TextView menu_in_h = Create_textview((main_width_ - 100), 100, 25, main_height_ / 3 - 250,
                head_b_in[id_menu], GravityFlags.Left, TypefaceStyle.Bold, 20);
            add_intake.AddView(menu_in_h);
            TextView menu_date_h = Create_textview((main_width_ - 100), 50, 25, main_height_ / 3 - 175,
                date_add, GravityFlags.Left, TypefaceStyle.Normal, 15);
            add_intake.AddView(menu_date_h);
            ImageView _image_intake = Create_imageview(main_width_ / 4 * 3, main_width_ / 4 * 3, (main_width_ - 100) / 3, (main_height_ / 3 - 100) / 3);
            _image_intake.SetImageResource(intake_im[id_menu]);
            add_intake.AddView(_image_intake);

            int ki = (main_height_ - (main_height_ / 3)) / 5;
            for (int j = 0; j < mass_intake.Count; j++)
            {
                FrameLayout add_i = Create_framelayout(main_width_ - 100, ki - 10, 50, (main_height_ / 3) + (ki * j), main_);
                add_i.Clickable = true;
                add_i.SetOnTouchListener(new OnSwipeTouchListener(add_i.Context, this, datadef, week, mass_intake_week, id_menu, mass_intake, (int)mass_intake[j].Id, user.calories));
                add_i.RemoveAllViews();
                add_i.SetBackgroundResource(Resource.Drawable.fram_add1);
                intake_menu.AddView(add_i);
                ImageView image_i = Create_imageview(ki - 20, ki - 20, 5, 5);
                //
                if (mass_intake[j].MainPicture != null)
                {
                    Bitmap bb = user.Convert_image_to(mass_intake[j].MainPicture);//week.RequestIntakeImage(mass_intake[j].Id);
                    image_i.SetImageBitmap(bb);
                }
                else image_i.SetImageResource(intake_im[id_menu]);
                TextView nm_i = Create_textview((main_width_ - ki) / 2, (ki - 10) / 2, ki + 10, 0,
                      mass_intake[j].Name, GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 20);
                TextView nm_i_k = Create_textview(main_width_ - 110, (ki - 10) / 2, 0, 0,
                      mass_intake[j].Colories.ToString() + " ккал", GravityFlags.Right | GravityFlags.CenterVertical, TypefaceStyle.Bold, 20);
                TextView nm_v_i = Create_textview(main_width_ - ki, (ki - 10) / 2, ki + 10, (ki - 10) / 2,
                      mass_intake[j].Weight.ToString() + " г		⊛		Б: " + mass_intake[j].Proteins.ToString() +
                      " г, Ж: " + mass_intake[j].Greases.ToString() + " г, У: " + mass_intake[j].Carbohydrates.ToString() + " г",
                      GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Normal, 15);

                add_i.AddView(image_i);
                add_i.AddView(nm_i);
                add_i.AddView(nm_i_k);
                add_i.AddView(nm_v_i);
            }

            basis.AddView(intake_menu);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            var display = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
            main_height_ = (int)display.Height - 277;
            main_width_ = (int)display.Width;
            main_ = Color.Rgb(255, 198, 24);
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Visibility = ViewStates.Invisible;
            fab.Click += FabOnClick;

            basis = FindViewById<RelativeLayout>(Resource.Id.basis);
            drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.SetCheckedItem(Resource.Id.nav_home);

            toolbar.SetBackgroundColor(main_);
            interface_fl = new List<FrameLayout>();
            user = new PersonalData();
            week = new MenuData();
            string path = System.IO.Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "userfile.txt");
            FileInfo ff = new FileInfo(path);
            if (ff.Exists)
            {
                user = user.FileRD();
                path = System.IO.Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "week_file.txt");
                ff = new FileInfo(path);
                if (ff.Exists)
                    week = week.FileRD();
                //if (week.data_update != DateTime.Now.ToString("dd.MM.yyyy"))
                {
                    week = week.RequestMenu(user.calories);
                    week.FileWR(week);
                }
            }
            else
            {
                Create_SettingFramelayout();
                week = week.RequestMenu(user.calories);
                week.FileWR(week);
            }
            string[] weekbt = new string[7] { "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ", "ВС" };
            for (int i = 0; i < 7; i++) if (weekbt[i].ToLower() == DateTime.Now.ToString("ddd")) CreateMenuFramelayout(i);
        }


        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                if (flagexit == false)
                {
                    flagexit = true;
                    ExitFlagNot();
                }
                else Process.KillProcess(Process.MyPid());
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            LinearLayout linearl = FindViewById<LinearLayout>(Resource.Id.linear_1);
            linearl.SetBackgroundResource(Resource.Drawable.fram_main1);
            ImageView image_navigation = FindViewById<ImageView>(Resource.Id.imageView);
            image_navigation.LayoutParameters = new LinearLayout.LayoutParams(300, 300);
            if (user.image != null) image_navigation.SetImageBitmap(user.Convert_image_to(user.image));
            else image_navigation.SetImageResource(Resource.Drawable.li);
            TextView text_navigation = FindViewById<TextView>(Resource.Id.texttView);
            text_navigation.TranslationX = 50;
            text_navigation.TextSize = 20;
            text_navigation.SetTextColor(Color.Black);
            text_navigation.Text = user.name;
            //MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            switch (item.ItemId)
            {
                case Resource.Id.nav_home:
                    string[] weekbt = new string[7] { "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ", "ВС" };
                    for (int i = 0; i < 7; i++) if (weekbt[i].ToLower() == DateTime.Now.ToString("ddd"))
                            CreateMenuFramelayout(i);
                    return true;
                case Resource.Id.nav_gallery:
                    Create_ProfileFramelayout();
                    return true;
                case Resource.Id.nav_manage:
                    Create_SettingFramelayout();
                    return true;
            }
            return false;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            //user.FileWR(user);
            //week.FileWR(week);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public class OnSwipeTouchListener : Java.Lang.Object, View.IOnTouchListener
        {
            private GestureDetector gestureDetector;
            public OnSwipeTouchListener(Context ctx, MainActivity ac, int data, MenuData wk, List<Recipe> _mw, int t_mt, List<Recipe> mw_add, int if_f, int cal)
            {
                gestureDetector = new GestureDetector(ctx, new GestureListener(ac, data, wk, _mw, t_mt, mw_add, if_f, cal));
            }

            public bool OnTouch(View v, MotionEvent e)
            {
                return gestureDetector.OnTouchEvent(e);
            }
        }
        public class GestureListener : GestureDetector.SimpleOnGestureListener
        {
            MainActivity ac_;
            int data_;
            MenuData wk_;
            List<Recipe> _mw_;
            int t_mt_;
            List<Recipe> mw_add_;
            int if_f_;
            int cal_;
            private static int SWIPE_THRESHOLD = 100;
            private static int SWIPE_VELOCITY_THRESHOLD = 100;
            public GestureListener(MainActivity ac, int dat, MenuData wk, List<Recipe> _mw, int t_mt, List<Recipe> mw_add, int if_f, int cal)
            {
                ac_ = ac;
                data_ = dat;
                wk_ = wk;
                _mw_ = _mw;
                t_mt_ = t_mt;
                mw_add_ = mw_add;
                if_f_ = if_f;
                cal_ = cal;
            }
            public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
            {
                // TODO
                bool result = false;
                try
                {
                    float diffY = e2.GetY() - e1.GetY();
                    float diffX = e2.GetX() - e1.GetX();
                    if (Math.Abs(diffX) > Math.Abs(diffY))
                    {
                        if (Math.Abs(diffX) > SWIPE_THRESHOLD && Math.Abs(velocityX) > SWIPE_VELOCITY_THRESHOLD)
                        {
                            if (diffX > 0)
                            {
                                onSwipeRight();
                            }
                            else
                            {
                                onSwipeLeft();
                            }
                            result = true;
                        }
                    }
                    else if (Math.Abs(diffY) > SWIPE_THRESHOLD && Math.Abs(velocityY) > SWIPE_VELOCITY_THRESHOLD)
                    {
                        if (diffY > 0)
                        {
                            onSwipeBottom();
                        }
                        else
                        {
                            onSwipeTop();
                        }
                        result = true;
                    }
                }
                catch (Exception exception)
                {
                    _ = exception.StackTrace;
                }
                return result;

            }
            public void onSwipeRight()
            {
               // Toast.MakeText(Application.Context, "right", ToastLength.Short).Show();
            }
            public void onSwipeLeft()
            {
                MainActivity ac = new MainActivity();
                //Toast.MakeText(Application.Context, "left " + int_id.ToString(), ToastLength.Short).Show();
                wk_.RequestChange(data_, _mw_, t_mt_, mw_add_, if_f_, cal_);
                ac_.CreateAddMenuFramelayot(data_, t_mt_);
            }
            public void onSwipeTop()
            {
               // Toast.MakeText(Application.Context, "top", ToastLength.Short).Show();
            }
            public void onSwipeBottom()
            {
               // Toast.MakeText(Application.Context, "up", ToastLength.Short).Show();
            }


        }
    }
}

