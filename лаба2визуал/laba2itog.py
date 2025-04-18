import pandas as pd #библиотека для обработки и анализа данных
import numpy as np #математическая библиотека с поддержкой многомерных массивов
import matplotlib.pyplot as plt #библиотека для построения графиков
import pycountry #библиотека для работы с данными о странах, включая их названия и коды
from matplotlib import cm
from mpl_toolkits.mplot3d import Axes3D

# Настройки для графиков
PLOT_LABEL_FONT_SIZE = 14 

# Генерация цветовой схемы
def getColors(n):
    COLORS = []
    cm = plt.cm.get_cmap('hsv', n)
    for i in np.arange(n):
        COLORS.append(cm(i))
    return COLORS

# Установка размера 2D графика
def set_plot_size(w,h,figure=plt):
    fig_size = plt.rcParams['figure.figsize']
    fig_size[0] = 12
    fig_size[1] = 4.5
    figure.rcParams['figure.figsize'] = fig_size
set_plot_size(12, 4.5)

# Сортировка словаря
def dict_sort(my_dict):
    keys = []
    values = []
    my_dict = sorted(my_dict.items(), key=lambda x:x[1], reverse=True)
    for k, v in my_dict:
        keys.append(k)
        values.append(v)
    return (keys,values)

# Чтение CSV файла
df = pd.read_csv('scrubbed.csv', escapechar='`', low_memory=False)

# Очистка данных
df.fillna(value='unknown', inplace=True)

# Анализ форм объектов
shapes_label_count = pd.value_counts(df['shape'].values, sort=True)

# Анализ стран
country_count = pd.value_counts(df['country'].values, sort=True)
country_count_keys, country_count_values = dict_sort(dict(country_count))    
TOP_COUNTRY = len(country_count_keys)
plt.title('Страны, где больше всего наблюдений', fontsize=PLOT_LABEL_FONT_SIZE)
plt.bar(np.arange(TOP_COUNTRY), country_count_values, color=getColors(TOP_COUNTRY))
plt.xticks(np.arange(TOP_COUNTRY), country_count_keys, rotation=0, fontsize=12)
plt.yticks(fontsize=PLOT_LABEL_FONT_SIZE) 
plt.ylabel('Количество наблюдений', fontsize=PLOT_LABEL_FONT_SIZE)
plt.show()

# Анализ типов объектов
shapes_type_count = pd.value_counts(df['shape'].values, sort=True)
shapes_type_count_keys, shapes_type_count_values = dict_sort(dict(shapes_type_count))   
OBJECT_COUNT = len(shapes_type_count_keys)
plt.title('Типы объектов', fontsize=PLOT_LABEL_FONT_SIZE)
bar = plt.bar(np.arange(OBJECT_COUNT), shapes_type_count_values, color=getColors(OBJECT_COUNT))
plt.xticks(np.arange(OBJECT_COUNT), shapes_type_count_keys, rotation=90, fontsize=PLOT_LABEL_FONT_SIZE)
plt.yticks(fontsize=PLOT_LABEL_FONT_SIZE)
plt.ylabel('Сколько раз видели', fontsize=PLOT_LABEL_FONT_SIZE)
plt.show()

# Анализ по месяцам
MONTH_COUNT = [0,0,0,0,0,0,0,0,0,0,0,0]
MONTH_LABEL = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
    'Июль', 'Август', 'Сентябрь' ,'Октябрь' ,'Ноябрь' ,'Декабрь']
for i in df['datetime']:
    m,d,y_t =  i.split('/')
    MONTH_COUNT[int(m)-1] += 1
plt.bar(np.arange(12), MONTH_COUNT, color=getColors(12))
plt.xticks(np.arange(12), MONTH_LABEL, rotation=90, fontsize=PLOT_LABEL_FONT_SIZE)
plt.ylabel('Частота появления', fontsize=PLOT_LABEL_FONT_SIZE)
plt.yticks(fontsize=PLOT_LABEL_FONT_SIZE)
plt.title('Частота появления объектов по месяцам', fontsize=PLOT_LABEL_FONT_SIZE)
plt.show()

# Анализ среднего времени появления объектов
shapes_durations_dict = {}
for i in shapes_type_count_keys:
     dfs = df[['duration (seconds)', 'shape']].loc[df['shape'] == i]   
     shapes_durations_dict[i] = dfs['duration (seconds)'].mean(axis=0)/60.0/60.0
    
shapes_durations_dict_keys = []
shapes_durations_dict_values = []
for k in shapes_type_count_keys:
    shapes_durations_dict_keys.append(k)
    shapes_durations_dict_values.append(shapes_durations_dict[k])
plt.title('Среднее время появление каждого объекта', fontsize=PLOT_LABEL_FONT_SIZE)
plt.bar(np.arange(OBJECT_COUNT), shapes_durations_dict_values, color=getColors(OBJECT_COUNT))
plt.xticks(np.arange(OBJECT_COUNT), shapes_durations_dict_keys, rotation=90, fontsize=PLOT_LABEL_FONT_SIZE)
plt.yticks(fontsize=PLOT_LABEL_FONT_SIZE)
plt.ylabel('Среднее время появления в часах', fontsize=PLOT_LABEL_FONT_SIZE)
plt.show()

# Анализ медианного времени появления объектов
shapes_durations_dict = {}
for i in shapes_type_count_keys:
     dfs = df[['duration (seconds)', 'shape']].loc[df['shape'] == i]   
     shapes_durations_dict[i] = dfs['duration (seconds)'].median(axis=0)/60.0/60.0    
shapes_durations_dict_keys = []
shapes_durations_dict_values = []
for k in shapes_type_count_keys:
    shapes_durations_dict_keys.append(k)
    shapes_durations_dict_values.append(shapes_durations_dict[k])
    
plt.title('Медианное время появление каждого объекта', fontsize=PLOT_LABEL_FONT_SIZE)
plt.bar(np.arange(OBJECT_COUNT), shapes_durations_dict_values, color=getColors(OBJECT_COUNT))
plt.xticks(np.arange(OBJECT_COUNT), shapes_durations_dict_keys, rotation=90, fontsize=PLOT_LABEL_FONT_SIZE)
plt.yticks(fontsize=PLOT_LABEL_FONT_SIZE)
plt.ylabel('Медианное время появления в часах', fontsize=PLOT_LABEL_FONT_SIZE)
plt.show()

#Анализ по городам
city_count = pd.value_counts(df['city'].values, sort=True)
city_count_keys, city_count_values = dict_sort(dict(city_count))
city_count_keys = city_count_keys[:20]
city_count_values = city_count_values[:20]
TOP_CITY = len(city_count_keys)
plt.title('Города, где больше всего наблюдений', fontsize=PLOT_LABEL_FONT_SIZE)
plt.bar(np.arange(TOP_CITY), city_count_values, color=getColors(TOP_CITY))
plt.xticks(np.arange(TOP_CITY), city_count_keys, rotation=90, fontsize=12)
plt.yticks(fontsize=PLOT_LABEL_FONT_SIZE) 
plt.ylabel('Количество наблюдений', fontsize=PLOT_LABEL_FONT_SIZE)
plt.show()

#Анализ по месяцам по отдельно взятой стране (Канада(Ca))
df_canada = df[df['country'] == 'ca']
MONTH_COUNT = [0,0,0,0,0,0,0,0,0,0,0,0]
MONTH_LABEL = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
    'Июль', 'Август', 'Сентябрь' ,'Октябрь' ,'Ноябрь' ,'Декабрь']
for i in df_canada['datetime']:
    m,d,y_t =  i.split('/')
    MONTH_COUNT[int(m)-1] += 1
plt.bar(np.arange(12), MONTH_COUNT, color=getColors(12))
plt.xticks(np.arange(12), MONTH_LABEL, rotation=90, fontsize=PLOT_LABEL_FONT_SIZE)
plt.ylabel('Частота появления', fontsize=PLOT_LABEL_FONT_SIZE)
plt.yticks(fontsize=PLOT_LABEL_FONT_SIZE)
plt.title('Частота появления объектов в Канаде по месяцам', fontsize=PLOT_LABEL_FONT_SIZE)
plt.show()

#Города Канады, где чаще всего наблюдались НЛО
canada_ufo_data = df[df['country'] == 'ca']
canada_city_counts = canada_ufo_data['city'].value_counts()
top_canada_cities = canada_city_counts.head(10)
top_canada_cities.plot(kind='bar', title='Топ-10 городов Канады, где наблюдают НЛО', color = getColors(12))
plt.show()

