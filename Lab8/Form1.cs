using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab8
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            storag.AddObserver(tree);
        }
        public class Figure
        {
            public int x, y;
            public bool is_drawed = true;
            public Color fillcolor = Color.Aqua;
            public Color color = Color.Black;
            public bool is_sticky = false;
            public Figure() { }
            public virtual string save() { return ""; }
            public virtual void load(string x, string y, string c, string fillcolor) { }
            public virtual void load(ref StreamReader sr, Figure figure, CreateFigure createFigure) { }
            public virtual void GroupAddFigure(ref Figure object1) { }
            public virtual void UnGroup(ref Storage stg, int c) { }
            public virtual void paint_figure(Pen pen, Panel paint_box) { }
            public virtual void move_x(int x, Panel paint_box) { }
            public virtual void move_y(int y, Panel paint_box) { }
            public virtual void changesize(int size) { }
            public virtual bool checkfigure(int x, int y) { return false; }
            public virtual void setcolor(Color color) { }
            public virtual void caseswitch(ref StreamReader sr, ref Figure figure, CreateFigure createFigure) { }
            public virtual void get_min_x(ref int f) { }
            public virtual void get_max_x(ref int f) { }
            public virtual void get_min_y(ref int f) { }
            public virtual void get_max_y(ref int f) { }
            public virtual string name() { return "null"; }
            public virtual void setcolored(Color color) { }

            public void setColor(Color color)
            {
                this.color = color;
            }
            public Color getColor()
            {
                return color;
            }
        }
        class Group : Figure
        {
            public int maxcount = 10;
            public Figure[] group;
            public int count;
            int min_x = 99999, max_x = 0, min_y = 99999, max_y = 0;
            public Group()
            {   // Выделяем maxcount мест в хранилище
                count = 0;
                group = new Figure[maxcount];
                for (int i = 0; i < maxcount; ++i)
                    group[i] = null;
            }
            public override string save()
            {   // Функция сохранения
                string str = "Group" + "\n" + count;
                for (int i = 0; i < count; ++i)
                    str += "\n" + group[i].save();
                return str;
            }
            public override void load(ref StreamReader sr, Figure figure, CreateFigure createFigure)
            {   // Функция загрузки
                int chislo = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < chislo; ++i)
                {
                    createFigure.caseswitch(ref sr, ref figure, createFigure);
                    GroupAddFigure(ref figure);
                }
            }
            public override void GroupAddFigure(ref Figure object1)
            {   // Добавляет фигуру в группу
                if (count >= maxcount)
                    return;
                group[count] = object1;
                ++count;
            }
            public override void UnGroup(ref Storage stg, int c)
            {   // Разгруппировка
                stg.delete_object(c);
                for (int i = 0; i < count; ++i)
                {
                    stg.add_object(index, ref group[i], k, ref indexin);
                }
            }
            public override void paint_figure(Pen pen, Panel paint_box)
            {   // Отображение группы
                for (int i = 0; i < count; ++i)
                {
                    group[i].paint_figure(pen, paint_box);
                }
            }
            public void getsize()
            {
                min_x = 99999; max_x = 0; min_y = 99999; max_y = 0;
                for (int i = 0; i < count; ++i)
                {
                    int f = 0;
                    group[i].get_min_x(ref f);
                    if (f < min_x)
                        min_x = f;
                    group[i].get_max_x(ref f);
                    if (f > max_x)
                        max_x = f;
                    group[i].get_min_y(ref f);
                    if (f < min_y)
                        min_y = f;
                    group[i].get_max_y(ref f);
                    if (f > max_y)
                        max_y = f;
                }
            }
            public override void move_x(int x, Panel paint_box)
            {   // Перемещение по оси x
                getsize();
                if ((min_x + x) > 0 && (max_x + x) < paint_box.ClientSize.Width)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        group[i].move_x(x, paint_box);
                    }
                }
            }
            public override void get_min_x(ref int f)
            {
                f = min_x;
            }
            public override void get_max_x(ref int f)
            {
                f = max_x;
            }
            public override void get_min_y(ref int f)
            {
                f = min_y;
            }
            public override void get_max_y(ref int f)
            {
                f = max_y;
            }
            public override void move_y(int y, Panel paint_box)
            {   // Перемещение по оси y
                getsize();
                if ((min_y + y) > 0 && (max_y + y) < paint_box.ClientSize.Height)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        group[i].move_y(y, paint_box);
                    }
                }
            }
            public override void changesize(int size)
            {   // Изменение размера
                for (int i = 0; i < count; ++i)
                {
                    group[i].changesize(size);
                }
            }
            public override bool checkfigure(int x, int y)
            {   // Проверка на фигуры
                for (int i = 0; i < count; ++i)
                {
                    if (group[i].checkfigure(x, y))
                        return true;
                }
                return false;
            }
            public override void setcolor(Color color)
            {   // Установка цвета
                for (int i = 0; i < count; ++i)
                {
                    group[i].setcolor(color);
                }
            }
            public override string name()
            {
                return "Group";
            }
            public override void setcolored(Color color)
            {
                for (int i = 0; i < count; ++i)
                {
                    group[i].color = color;
                }
                this.color = color;
            }
        }
    
        class Circle : Figure
        {
            public int rad = 30; // Радиус круга
            public Circle() { }
            public Circle(int x, int y)
            {
                this.x = x - rad;
                this.y = y - rad;
            }
            ~Circle() { }
            public override string save()
            {   // Функция сохранения
                return "Circle" + "\n" + x + "\n" + y + "\n" + rad + "\n" + fillcolor.ToArgb().ToString();
            }
            public override void load(string x, string y, string rad, string fillcolor)
            {   // Функция загрузки
                this.x = Convert.ToInt32(x);
                this.y = Convert.ToInt32(y);
                this.rad = Convert.ToInt32(rad);
                this.fillcolor = Color.FromArgb(Convert.ToInt32(fillcolor));
            }
            public override void paint_figure(Pen pen, Panel paint_box)
            {   // Отображение фигуры
                SolidBrush figurefillcolor = new SolidBrush(fillcolor);
                paint_box.CreateGraphics().DrawEllipse(
                    pen, x, y, rad * 2, rad * 2);
                paint_box.CreateGraphics().FillEllipse(
                    figurefillcolor, x, y, rad * 2, rad * 2);
            }
            public override void get_min_x(ref int f)
            {
                f = x;
            }
            public override void get_max_x(ref int f)
            {
                f = x + (rad * 2);
            }
            public override void get_min_y(ref int f)
            {
                f = y;
            }
            public override void get_max_y(ref int f)
            {
                f = y + (rad * 2);
            }
            public override void move_x(int x, Panel paint_box)
            {   // Перемещение по оси x
                int c = this.x + x;
                int gran = paint_box.ClientSize.Width - (rad * 2);
                check(c, x, gran, gran - 2, ref this.x);
            }
            public override void move_y(int y, Panel paint_box)
            {   // Перемещение по оси y
                int c = this.y + y;
                int gran = paint_box.ClientSize.Height - (rad * 2);
                check(c, y, gran, gran - 2, ref this.y);
            }
            public override void changesize(int size)
            {   // Изменение размера
                rad += size;
            }
            public override bool checkfigure(int x, int y)
            {   // Проверка на фигуры
                return ((x - this.x - rad) * (x - this.x - rad) + (y - this.y - rad) *
                    (y - this.y - rad)) < (rad * rad);
            }
            public override void setcolor(Color color)
            {   // Установка цвета
                fillcolor = color;
            }
            public override string name()
            {
                return "Circle";
            }
            public override void setcolored(Color color)
            {
                this.color = color;
            }
        }
        class Square : Figure
        {
            public int x2, y2, size = 60;

            public Square() { }
            public Square(int x, int y)
            {
                this.x = x - size / 2;
                this.y = y - size / 2;
                this.x2 = size;
                this.y2 = size;
            }

            ~Square() { }

            public override string save()
            {   // Функция сохранения
                return "Square" + "\n" + x + "\n" + y + "\n" + size + "\n" + fillcolor.ToArgb().ToString();
            }
            public override void load(string x, string y, string size, string fillcolor)
            {   // Функция загрузки
                this.x = Convert.ToInt32(x);
                this.y = Convert.ToInt32(y);
                this.size = Convert.ToInt32(size);
                this.fillcolor = Color.FromArgb(Convert.ToInt32(fillcolor));
            }
            public override void paint_figure(Pen pen, Panel paint_box)
            {   // Отображение фигуры
                SolidBrush figurefillcolor = new SolidBrush(fillcolor);
                paint_box.CreateGraphics().DrawRectangle(pen,
                    x, y, size, size);
                paint_box.CreateGraphics().FillRectangle(figurefillcolor,
                    x, y, size, size);
            }
            public override void get_min_x(ref int f)
            {
                f = x;
            }
            public override void get_max_x(ref int f)
            {
                f = x + size;
            }
            public override void get_min_y(ref int f)
            {
                f = y;
            }
            public override void get_max_y(ref int f)
            {
                f = y + size;
            }
            public override void move_x(int x, Panel paint_box)
            {   // Перемещение по оси x
                int s = this.x + x;
                int gran = paint_box.ClientSize.Width - size;
                check(s, x, gran, --gran, ref this.x);
            }
            public override void move_y(int y, Panel paint_box)
            {   // Перемещение по оси y
                int s = this.y + y;
                int gran = paint_box.ClientSize.Height - size;
                check(s, y, gran, --gran, ref this.y);
            }
            public override void changesize(int size)
            {   // Изменение размера
                this.size += size;
            }
            public override bool checkfigure(int x, int y)
            {   // Проверка на фигуры
                return (this.x <= x && x <= (this.x + size) &&
                                        this.y <= y && y <= (this.y + size));
            }
            public override void setcolor(Color color)
            {   // Установка цвета
                fillcolor = color;
            }
            public override string name()
            {
                return "Square";
            }
            public override void setcolored(Color color)
            {
                this.color = color;
            }
        }
        class Line : Figure
        {
            public int lenght = 60, wight = 5;
            public Line() { }
            public Line(int x, int y)
            {
                this.x = x - lenght / 2;
                this.y = y;
            }
            ~Line() { }
            public override string save()
            {   // Функция сохранения
                return "Line" + "\n" + x + "\n" + y + "\n" + lenght + "\n" + fillcolor.ToArgb().ToString();
            }
            public override void load(string x, string y, string lenght, string fillcolor)
            {   // Функция загрузки
                this.x = Convert.ToInt32(x);
                this.y = Convert.ToInt32(y);
                this.lenght = Convert.ToInt32(lenght);
                this.fillcolor = Color.FromArgb(Convert.ToInt32(fillcolor));
            }
            public override void paint_figure(Pen pen, Panel paint_box)
            {   // Отображение фигуры
                SolidBrush figurefillcolor = new SolidBrush(fillcolor);
                paint_box.CreateGraphics().DrawRectangle(pen, x,
                                        y, lenght, wight);
                paint_box.CreateGraphics().FillRectangle(figurefillcolor, x,
                    y, lenght, wight);
            }
            public override void get_min_x(ref int f)
            {
                f = x;
            }
            public override void get_max_x(ref int f)
            {
                f = x + lenght;
            }
            public override void get_min_y(ref int f)
            {
                f = y;
            }
            public override void get_max_y(ref int f)
            {
                f = y + wight;
            }
            public override void move_x(int x, Panel paint_box)
            {   // Перемещение по оси x
                int l = this.x + x;
                int gran = paint_box.ClientSize.Width - lenght;
                check(l, x, gran, --gran, ref this.x);
            }
            public override void move_y(int y, Panel paint_box)
            {   // Перемещение по оси y
                int l = this.y + y;
                int gran = paint_box.ClientSize.Height - wight;
                check(l, y, gran, --gran, ref this.y);
            }
            public override void changesize(int size)
            {   // Изменение размера
                lenght += size;
            }
            public override bool checkfigure(int x, int y)
            {   // Проверка на фигуры
                return (this.x <= x && x <= (this.x + lenght) && (this.y - 2) <= y &&
                                    y <= (this.y + wight));
            }
            public override void setcolor(Color color)
            {   // Установка цвета
                fillcolor = color;
            }
            public override string name()
            {
                return "Line";
            }
            public override void setcolored(Color color)
            {
                this.color = color;
            }
        }
        public class CreateFigure : Figure
        {   // Используем Factory Method
            public override void caseswitch(ref StreamReader sr, ref Figure figure, CreateFigure createFigure)
            {
                string str = sr.ReadLine();
                switch (str)
                {   // В зависимости какая фигура выбрана
                    case "Circle":
                        figure = new Circle();
                        figure.load(sr.ReadLine(), sr.ReadLine(), sr.ReadLine(), sr.ReadLine());
                        break;
                    case "Line":
                        figure = new Line();
                        figure.load(sr.ReadLine(), sr.ReadLine(), sr.ReadLine(), sr.ReadLine());
                        break;
                    case "Square":
                        figure = new Square();
                        figure.load(sr.ReadLine(), sr.ReadLine(), sr.ReadLine(), sr.ReadLine());
                        break;
                    case "Group":
                        figure = new Group();
                        figure.load(ref sr, figure, createFigure);
                        break;
                }
            }
        }
        public class Storage:IObservable
        {
            public Figure[] objects;
            public TreeView treeView;
            public List<IObserver> observers;

            public Storage(int count)
            {   // Конструктор по умолчанию 
                // Выделяем count мест в хранилище
                objects = new Figure[count];
                observers = new List<IObserver>();
                for (int i = 0; i < count; ++i)
                    objects[i] = null;
            }
            public void intit_tree(ref TreeView treeView)
            {
                this.treeView = treeView;
            }
            public void initialisat(int count)
            {   // Выделяем count мест в хранилище
                objects = new Figure[count];
                for (int i = 0; i < count; ++i)
                    objects[i] = null;
            }
            public void add_object(int ind, ref Figure object1, int count, ref int indexin)
            {   // Добавляет ячейку в хранилище
                // Если ячейка занята ищем свободное место
                while (objects[ind] != null)
                {
                    ind = (ind + 1) % count;
                }
                objects[ind] = object1;
                indexin = ind;
                sorting(k);
                NotifyObservers();
            }
            public void delete_object(int ind)
            {   // Удаляет объект из хранилища
                objects[ind] = null;
                if (index > 0)
                    index--;
                NotifyObservers();
            }

            public bool check_empty(int index)
            {   // Проверяет занято ли место хранилище
                if (objects[index] == null)
                    return true;
                else return false;
            }

            public int occupied(int size)
            { // Определяет кол-во занятых мест в хранилище
                int count_occupied = 0;
                for (int i = 0; i < size; ++i)
                    if (!check_empty(i))
                        ++count_occupied;
                return count_occupied;
            }

            public void doubleSize(ref int size)
            {   // Функция для увеличения кол-ва элементов в хранилище в 2 раза 
                Storage storage1 = new Storage(size * 2);
                for (int i = 0; i < size; ++i)
                    storage1.objects[i] = objects[i];
                for (int i = size; i < (size * 2) - 1; ++i)
                {
                    storage1.objects[i] = null;
                }
                size = size * 2;
                initialisat(size);
                for (int i = 0; i < size; ++i)
                    objects[i] = storage1.objects[i];
            }
            public void AddObserver(IObserver o)
            {
                observers.Add(o);
            }
            public void RemoveObserver(IObserver o)
            {
                observers.Remove(o);
            }
            public void NotifyObservers()
            {
                foreach (IObserver observer in observers)
                    observer.Update(ref treeView, this);
            }
            public void sorting(int size)
            {
                Storage storage1 = new Storage(size);
                int col = 0;
                for (int i = 0; i < size; ++i)
                {
                    if (!check_empty(i))
                    {
                        storage1.objects[col] = objects[i];
                        ++col;
                    }
                }
                initialisat(size);
                for (int i = 0; i < size; ++i)
                    objects[i] = storage1.objects[i];
            }
            ~Storage() { }
        };
        class TreeViews : IObserver
        {
            public TreeViews() { }
            public void Update(ref TreeView treeView, Storage stg)
            {   // Перерисовка treeView
                treeView.Nodes.Clear();
                treeView.Nodes.Add("Фигуры");
                for (int i = 0; i < k; ++i)
                {
                    if (!stg.check_empty(i))
                    {
                        fillnode(treeView.Nodes[0], stg.objects[i]);
                    }
                }
                treeView.ExpandAll();
            }
            public void treeSelect(ref TreeView treeView, int index) //выбор узла
            {   // Выделяем узел
                treeView.SelectedNode = treeView.Nodes[0].Nodes[index];
                treeView.Focus();
            }
            public void fillnode(TreeNode treeNode, Figure figure)
            {   // Заполняем treeView
                TreeNode nodes = treeNode.Nodes.Add(figure.name());
                if (figure.name() == "Group")
                {
                    for (int i = 0; i < (figure as Group).count; ++i)
                    {
                        fillnode(nodes, (figure as Group).group[i]);
                    }
                }
            }

        }
        private void paint_box_MouseMove(object sender, MouseEventArgs e)
        {
            label_x.Text = "X: " + e.X.ToString();
            label_y.Text = "Y: " + e.Y.ToString();
        }
        private void paint_Figure(Color name, ref Storage stg, int index)
        {
            if (!stg.check_empty(index))
            {
                Pen pen = new Pen(name, 3);
                stg.objects[index].setColor(name);
                stg.objects[index].paint_figure(pen, paint_box);
            }
        }
        private void remove_selection_circle(ref Storage stg)
        {   // Снимает выделение у всех элементов хранилища
            for (int i = 0; i < k; ++i)
            {
                if (!storag.check_empty(i))
                {
                    paint_Figure(Color.Black, ref storag, i);
                }
            }
        }
        private void remove_selected_circle(ref Storage stg)
        {   // Удаляет выделенные элементы из хранилища
            for (int i = 0; i < k; ++i)
            {
                if (!storag.check_empty(i))
                {
                    if (storag.objects[i].getColor() == Color.White)
                    {
                        storag.delete_object(i);
                    }
                }
            }
        }

        int p = 0; // Нажат ли был ранее Ctrl
        static int k = 5; // Кол-во ячеек в хранилище
        Storage storag = new Storage(k); // Создаем объект хранилища
        static int index = 0; // Кол-во нарисованных кругов
        static int indexin = 0; // Индекс, в какое место был помещён круг
        int size = 0;
        string path = @"C:\Users\ilya\source\repos\OOP\Lab7\Lab7\Storage.txt";

        private int check_figure(ref Storage stg, int size, int x, int y)
        {   // Проверяет есть ли уже фигура с такими же координатами в хранилище
            if (stg.occupied(size) != 0)
            {
                for (int i = 0; i < size; ++i)
                {
                    if (!stg.check_empty(i))
                    {   // Если под i индексом в хранилище есть объект
                        if (stg.objects[i].checkfigure(x, y))
                            return i;
                    }
                }
            }
            return -1;
        }
        private void button_clear_paintbox_Click(object sender, EventArgs e)
        {   // Очищает панель от кругов
            paint_box.Refresh(); // Перерисовывем панель paint_box

            for (int i = 0; i < k; ++i)
            {
                if (!storag.check_empty(i))
                {   // Меняем is_drawed на false
                    storag.objects[i].is_drawed = false;
                }
            }
        }
        private void button_show_Click(object sender, EventArgs e)
        {   // Отобразить все круги из хранилища
            paint_box.Refresh();
            if (storag.occupied(k) != 0)
            {

                for (int i = 0; i < k; ++i)
                {
                    if (!storag.check_empty(i))
                    {   // Меняем is_drawed на true
                        storag.objects[i].is_drawed = true;
                    }
                    paint_Figure(Color.Aqua, ref storag, i);
                }
            }

        }
        private void button_deletestorage_Click(object sender, EventArgs e)
        {   // Удалить все круги из хранилища
            for (int i = 0; i < k; ++i)
            {
                storag.objects[i] = null;
            }
            index = 0;
        }
        private void paint_box_MouseClick(object sender, MouseEventArgs e)
        {
            //Проверка на наличие круга на данных координатах
            int c = check_figure(ref storag, k, e.X, e.Y);
            storag.intit_tree(ref treeView1);
            if (index == k)
                storag.doubleSize(ref k);
            if (c != -1)
            {   // Если на этом месте уже нарисован круг
                if (Control.ModifierKeys == Keys.Control)
                {   // Если нажат ctrl, то выделяем несколько объектов
                    if (p == 0)
                    {
                        paint_Figure(Color.Black, ref storag, indexin);
                        p = 1;
                    }
                    // Вызываем функцию отрисовки круга                            
                    paint_Figure(Color.White, ref storag, c);
                }
                else
                {   // Иначе выделяем только один объект
                    // Снимаем выделение у всех объектов хранилища
                    remove_selection_circle(ref storag);
                    // Вызываем функцию отрисовки круга                   
                    // Вызываем функцию отрисовки круга                          
                    paint_Figure(Color.White, ref storag, c);
                    tree.treeSelect(ref treeView1, c);
                }
                return;
            }

            else
            {
                if (rb_Circle.Checked == true)
                {
                    Figure krug = new Circle(e.X, e.Y);// Добавляем круг в хранилище                    
                    storag.add_object(index, ref krug, k, ref indexin); // Снимаем выделение у всех объектов хранилища
                    remove_selection_circle(ref storag); // Вызываем функцию отрисовки круга
                    storag.objects[indexin].fillcolor = Color.Aqua;
                    paint_Figure(Color.Black, ref storag, indexin);
                    ++index;
                }
                else
                {
                    if (rb_Square.Checked == true)
                    {
                        Figure kvadrat = new Square(e.X, e.Y);
                        storag.add_object(index, ref kvadrat, k, ref indexin);
                        remove_selection_circle(ref storag);
                        storag.objects[indexin].fillcolor = Color.Aqua;
                        paint_Figure(Color.Black, ref storag, indexin);
                        ++index;
                    }
                    else
                    {
                        if (rb_Line.Checked == true)
                        {
                            Figure liniya = new Line(e.X, e.Y);
                            storag.add_object(index, ref liniya, k, ref indexin);
                            remove_selection_circle(ref storag);
                            storag.objects[indexin].fillcolor = Color.Aqua;
                            paint_Figure(Color.Black, ref storag, indexin);
                            ++index;
                        }
                    }
                }
            }
            p = 0;
        }
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {   // Удаление выделенных фигур, если нажата кнопка delete
                remove_selected_circle(ref storag);
            }
            if (e.KeyCode == Keys.W)
            {   // Перемещение по оси X вверх
                move_y(ref storag, -10);
            }
            if (e.KeyCode == Keys.S)
            {   // Перемещение по оси X вниз
                move_y(ref storag, +10);
            }
            if (e.KeyCode == Keys.A)
            {   // Перемещение по оси Y вниз
                move_x(ref storag, -10);
            }
            if (e.KeyCode == Keys.D)
            {   // Перемещение по оси Y вверх
                move_x(ref storag, +10);
            }
            paint_box.Refresh();
            paint_all(ref storag);
        }
        private void rb_Aqua_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < k; ++i)
            {
                if (!storag.check_empty(i))
                {
                    if (storag.objects[i].getColor() == Color.White)
                    {
                        storag.objects[i].setcolor(Color.Aqua);
                        paint_Figure(Color.Black, ref storag, i);
                    }
                }
            }
        }
        private void rb_Green_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < k; ++i)
            {
                if (!storag.check_empty(i))
                    if (storag.objects[i].getColor() == Color.White)
                    {
                        storag.objects[i].setcolor(Color.Green);
                        paint_Figure(Color.Black, ref storag, i);
                    }
            }
        }
        private void rb_Red_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < k; ++i)
            {
                if (!storag.check_empty(i))
                {
                    if (storag.objects[i].getColor() == Color.White)
                    {
                        storag.objects[i].setcolor(Color.Red);
                        paint_Figure(Color.Black, ref storag, i);
                    }
                }
            }
        }
        private void paint_all(ref Storage stg)
        {   // Рисует все фигуры на панели
            for (int i = 0; i < k; ++i)
                if (!stg.check_empty(i))
                    paint_Figure(stg.objects[i].getColor(), ref storag, i);
        }
        private void move_y(ref Storage stg, int y)
        {   // Функция для перемещения фигур по оси Y
            for (int i = 0; i < k; ++i)
            {
                if (!stg.check_empty(i))
                {
                    if (stg.objects[i].getColor() == Color.White)
                    {   // Если объект выделен
                        stg.objects[i].move_y(y, paint_box);
                    }
                }
            }
        }
        private void move_x(ref Storage stg, int x)
        {   // Функция для перемещения фигур по оси X
            for (int i = 0; i < k; ++i)
            {
                if (!stg.check_empty(i))
                {
                    if (stg.objects[i].getColor() == Color.White)
                    {   // Если объект выделен
                        stg.objects[i].move_x(x, paint_box);
                    }
                }
            }
        }
        static public void check(int f, int chislo, int gran, int gran1, ref int x)
        {   // Проверка на выход фигуры за границы
            if (f > 0 && f < gran)
                x += chislo;
            else
            {
                if (f <= 0)
                    x = 1;
                else
                    if (f >= gran1)
                    x = gran1;
            }
        }
        private void changesize(ref Storage stg, int size)
        {   // Увеличивает или уменьшает размер фигур, в зависимости от size
            for (int i = 0; i < k; ++i)
            {
                if (!stg.check_empty(i))
                {   // Если под i индексом в хранилище есть объект
                    if (stg.objects[i].getColor() == Color.White)
                    {
                        stg.objects[i].changesize(size);
                    }
                }
            }
        }
        private void btn_Minus_Click(object sender, EventArgs e)
        {
            size = -5;
            changesize(ref storag, size);
            paint_box.Refresh();
            paint_all(ref storag);
        }
        private void btn_Plus_Click(object sender, EventArgs e)
        {
            size = 5;
            changesize(ref storag, size);
            paint_box.Refresh();
            paint_all(ref storag);
        }
        private void btn_Group_Click(object sender, EventArgs e)
        {   // Создаём группу из выделенных фигур
            Figure group = new Group();
            for (int i = 0; i < k; ++i)
            {
                if (!storag.check_empty(i))
                    if (storag.objects[i].getColor() == Color.White)
                    {
                        group.GroupAddFigure(ref storag.objects[i]);
                        storag.delete_object(i);
                    }
            }
            storag.add_object(index, ref group, k, ref indexin);
        }
        private void btn_Ungroup_Click(object sender, EventArgs e)
        {   // Разгруппировка группы
            for (int i = 0; i < k; ++i)
            {
                if (!storag.check_empty(i))
                    if (storag.objects[i].getColor() == Color.White)
                    {
                        storag.objects[i].UnGroup(ref storag, i);
                        return;
                    }
            }
            paint_all(ref storag);
        }
        private void btn_Save_Click(object sender, EventArgs e)
        {   // Сохраяем хранилище в файл
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(storag.occupied(k));
                for (int i = 0; i < k; ++i)
                {
                    if (!storag.check_empty(i))
                    {
                        sw.WriteLine(storag.objects[i].save());
                    }
                }
            }
        }
        private void btn_Load_Click(object sender, EventArgs e)
        {   // Загружаем данные из файла
            StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
            {
                string str = sr.ReadLine();
                int strend = Convert.ToInt32(str);
                for (int i = 0; i < strend; ++i)
                {
                    Figure figure = new Figure();
                    CreateFigure create = new CreateFigure();
                    create.caseswitch(ref sr, ref figure, create);
                    if (index == k)
                        storag.doubleSize(ref k);
                    storag.add_object(index, ref figure, k, ref indexin);
                    ++index;
                }
                paint_all(ref storag);
                sr.Close();
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Выделение фигур из treeView
            remove_selection_circle(ref storag);
            int g;
            if (e.Node.Level != 1)
                g = e.Node.Parent.Index;
            else
                g = e.Node.Index;
            paint_Figure(Color.White, ref storag, g);
        }

        TreeViews tree = new TreeViews();
        int figure_now = 1; // Какая фигура выбрана	
        public interface IObservable
        {   // Наблюдаемый объект
            void AddObserver(IObserver o);
            void RemoveObserver(IObserver o);
            void NotifyObservers();
        }
        public interface IObserver
        {   // Наблюдатель
            void Update(ref TreeView treeView, Storage stg);
        }
    }
}
