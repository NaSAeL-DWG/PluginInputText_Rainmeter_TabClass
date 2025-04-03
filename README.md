# PluginInputText_Rainmeter_TabClass
---
Rainmeter插件InputText的修改版，用于支持 TabClass 3 的缩放功能。 A modified version of RainmeterPlugin:InputText . In order to support the scale funtion of TabClass 3 Skin.
---
 添加一个新参数 Scale 用于缩放 , 在Input Measure中添加 Scale 参数来使用，默认1.0，它会将你设置的XYWH和FontSize进行倍乘，支持小数，但请注意从代码上看XYWH会被取整。
 关于该插件的其他用法，还请参考该插件的 [官方文档](https://docs.rainmeter.net/manual/plugins/inputtext/)
## 使用
在此通过展示TabClass 3的有关代码片段以说明使用方法：
```
[Changeinput4]
Measure=Plugin
Plugin=InputTextC
Hidden=1
AntiAlias=1
X=343
Y=538
W=29
H=29
Scale=#scale#
FontSize=14
```
插件会自动根据Scale的参数对输入的坐标进行倍乘，支持变量。

## 构建
### 1.自行编译
> [!NOTE]
> 推荐使用 Visual Studio 2022 或该软件的其他版本进行操作
- 从[Rainmeter 存储库](https://github.com/rainmeter/rainmeter)中 下载 完整的源代码，解压后存放在固定位置
- 下载本存储库中 PluginInputText 文件夹中的所有内容，将其替换到 rainmeter-master\Plugins\PluginInputText 文件夹中
- 使用 Visual Studio 打开 Rainmeter.sln
- 在解决方案资源管理器中找到PluginInputText，右键选择 生成
- 找到其输出的文件，放入到Rainmeter程序的Plugin目录即可（记得改个名字）
### 2.直接使用Release
- 下载最新发布的Release中的 InputTextC.dll 放入到Rainmeter程序的Plugin目录即可

## 问题
尚不清楚存在的问题，若发现欢迎提交Issue

## 许可证
该项目使用GPL-2.0 许可证，原因是原项目同样使用该许可证许可，若有任何问题请提交Issue





