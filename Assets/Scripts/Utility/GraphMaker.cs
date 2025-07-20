using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphMaker : MonoBehaviour
{
    /// <summary>
    /// The material used for the line renderers in the graph.
    /// </summary>
    [SerializeField] private Material lineMaterial;

    /// <summary>
    /// The width of the lines in the graph.
    /// </summary>
    [SerializeField] private float lineWidth = 0.1f;

    /// <summary>
    /// The default font size for the graph text elements.
    /// </summary>
    [SerializeField] float fontSize = 3f;

    /// <summary>
    /// The offset for the color of the graph lines.
    /// </summary>
    [SerializeField] private float colorOffset = 0.1f;

    /// <summary>
    /// X axis line renderer for the graph.
    /// </summary>
    private LineRenderer xLineRenderer;

    /// <summary>
    /// Y axis line renderer for the graph.
    /// </summary>
    private LineRenderer yLineRenderer;

    /// <summary>
    /// all plot points for the graph.
    /// </summary>
    private Vector3[][] graphPlots;

    /// <summary>
    /// The graph canvas that contains the graph text elements.
    /// </summary>
    private GameObject canvas;

    /// <summary>
    /// Origin point of the graph in world space.
    /// </summary>
    private Transform origin;

    /// <summary>
    /// The offset for numbers displayed on the graph.
    /// </summary>
    public Vector2 NumberOffset = new Vector2(-3, -3);

    /// <summary>
    /// Represents the number of steps along the X and Y axes.
    /// </summary>
    /// <remarks>This field is used to define a grid or resolution in a two-dimensional space,  where the X
    /// and Y components specify the number of divisions along each axis.</remarks>
    public Vector2Int steps = new Vector2Int(10, 10);

    /// <summary>
    /// Range of the population values to be displayed on the graph.
    /// </summary>
    public Vector2 range = new Vector2(100, 100);

    /// <summary>
    /// Size of the graph in the world space.
    /// </summary>
    public Vector2 size = new Vector2(10, 10);

    /// <summary>
    /// scale factor for the axis based on the size and range of the graph.
    /// </summary>
    /// 
    public Vector2 graphScale => new Vector2(size.x / range.x, size.y / range.y);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        origin = transform.Find("Origin Point");

        canvas = origin.Find("Canvas").gameObject;

        yLineRenderer = origin.Find("Line Renders/Y Line Renderer").GetComponent<LineRenderer>();
        xLineRenderer = origin.Find("Line Renders/X Line Renderer").GetComponent<LineRenderer>();

        UpdateGraphMetrics();

        Vector3[] testpoints = new Vector3[10]
        {
                Vector3.zero,
                new Vector3(10, 10),
                new Vector3(20, 20),
                new Vector3(30, 30),
                new Vector3(40, 40),
                new Vector3(50, 50),
                new Vector3(60, 60),
                new Vector3(70, 70),
                new Vector3(80, 80),
                new Vector3(90, 90)
        };

        Vector3[] testpointstwo = new Vector3[10]
        {
                Vector3.zero,
                new Vector3(00, 10),
                new Vector3(10, 20),
                new Vector3(20, 30),
                new Vector3(30, 40),
                new Vector3(40, 50),
                new Vector3(50, 60),
                new Vector3(60, 70),
                new Vector3(70, 80),
                new Vector3(80, 90)
        };

        Vector3[][] testpointsArray = new Vector3[2][]
            {
                testpoints,
                testpointstwo
            };

        SetPlotPoints(testpointsArray);
    }

    /// <summary>
    /// Sets the plot points for the graph.
    /// </summary>
    /// <remarks>This method updates the graph with the provided plot points.  The graph will be redrawn to
    /// reflect the new data.</remarks>
    /// <param name="points">A two-dimensional array of <see cref="Vector3"/> objects representing the plot points.  Each inner array defines
    /// a series of points for a single plot.</param>
    public void SetPlotPoints(Vector3[][] points)
    {
        graphPlots = points;

        UpdateGraphPlot();
    }

    /// <summary>
    /// Updates the graph by generating and positioning numeric labels along the X and Y axes.
    /// </summary>
    /// <remarks>This method creates numeric labels for both the X and Y axes of the graph based on the
    /// specified range and step values. The labels are positioned relative to the graph's origin and scaled according
    /// to the size of the graph. The X-axis labels are offset vertically, while the Y-axis labels are offset
    /// horizontally.</remarks>
    public void UpdateGraphMetrics()
    {
        for (int x = 0; x < steps.x + 1; x++)
        {
            float xValue = range.x / steps.x * x;

            GameObject currentNumber = new GameObject($"X Number {x}");
            currentNumber.transform.SetParent(canvas.transform);
        
            TextMeshPro tmpText = currentNumber.AddComponent<TextMeshPro>();
            tmpText.fontSize = fontSize;
            tmpText.autoSizeTextContainer = true;
            tmpText.color = Color.black;
            tmpText.text = xValue.ToString();

            // X-axis label (moves along X, offset down/up)
            currentNumber.transform.localPosition = new Vector3(xValue * graphScale.x, NumberOffset.x, 0f);
        }

        for (int y = 1; y < steps.y + 1; y++)
        {
            float yValue = range.y / steps.y * y;

            GameObject currentNumber = new GameObject($"Y Number {y}");
            currentNumber.transform.SetParent(canvas.transform);

            TextMeshPro tmpText = currentNumber.AddComponent<TextMeshPro>();
            tmpText.color = Color.black;
            tmpText.autoSizeTextContainer = true;
            tmpText.fontSize = fontSize;
            tmpText.text = yValue.ToString();

            // Y-axis label (moves along Y, offset left/right)
            currentNumber.transform.localPosition = new Vector3(NumberOffset.y, yValue * graphScale.y, 0f);
        }

        yLineRenderer.positionCount = 2;
        xLineRenderer.positionCount = 2;

        yLineRenderer.SetPosition(0, new Vector3(0f, 0f, 0f));
        yLineRenderer.SetPosition(1, new Vector3(0f, range.y * graphScale.y, 0f));

        xLineRenderer.SetPosition(0, new Vector3(0f, 0f, 0f));
        xLineRenderer.SetPosition(1, new Vector3(range.x * graphScale.x, 0f, 0f));
    }

    /// <summary>
    /// Updates the graph plot by creating line renderers for each set of plot points.
    /// </summary>
    private void UpdateGraphPlot()
    {
        ClearLineRenderers();

        for (int i = 0; i < graphPlots.Length; i++)
        {
            GameObject lineRendererObject = new GameObject($"Line Renderer {i}");
            lineRendererObject.transform.SetParent(origin.Find("Line Renders"));
            lineRendererObject.transform.position = origin.position;

            Vector3[] newPlots = GetScaledPlot(graphPlots[i]);

            LineRenderer lineRenderingComponent = lineRendererObject.AddComponent<LineRenderer>();
            lineRenderingComponent.material = lineMaterial;
            lineRenderingComponent.startWidth = lineWidth;
            Color color = Color.HSVToRGB(((float)i / graphPlots.Length) + colorOffset, 1f, 1f);

            lineRenderingComponent.colorGradient = new Gradient
            {
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey(color, 0f),
                    new GradientColorKey(color, 1f)
                }
            };

            lineRenderingComponent.positionCount = newPlots.Length;
            lineRenderingComponent.SetPositions(newPlots);
        }
    }

    private Vector3[] GetScaledPlot(Vector3[] originalPlot)
    {
        Vector3[] scaled = new Vector3[originalPlot.Length];

        for (int i = 0; i < originalPlot.Length; i++)
        {
            scaled[i] = new Vector3(
                originalPlot[i].x * graphScale.x + origin.position.x,
                originalPlot[i].y * graphScale.y + origin.position.y,
                0f
            );
        }

        return scaled;
    }

    /// <summary>
    /// Clears all line renderers from the graph.
    /// </summary>
    public void ClearLineRenderers()
    {
        Transform lineParent = origin.Find("Line Renders/Line Graph");

        foreach (Transform child in lineParent)
        {
            Destroy(child.gameObject);
        }
    }
}
