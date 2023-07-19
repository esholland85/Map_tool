public static class WindowManager
{
    private static int wm_NextId = 2222;
    // Update is called once per frame

    public static int GetWindowId()
    {
        return wm_NextId++;
    }
}
